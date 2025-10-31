using AutoMapper;
using BookStore.Business.Dto;
using BookStore.Business.Helper;
using BookStore.Business.Service.Interface;
using BookStore.Data.Entity;
using BookStore.Data.Helper;
using BookStore.Data.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Implement
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public AuthenticationService(IRepository<User> userRepository,
            IConfiguration config,
            IMapper mapper)
        {
            _config = config;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ResponseMessage<AuthResponse>> Login(LoginRequest requestAuthDTO)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(
                x => x.Email.ToLower() == requestAuthDTO.email.ToLower(),
                isTracking: true
            );

            if (user == null)
                throw new NotFoundException("Email not found");

            bool validPassword = BCrypt.Net.BCrypt.Verify(requestAuthDTO.password, user.HashPassword);

            if (!validPassword)
                throw new NotFoundException("Wrong password");

            var (accessToken, expireAt) = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return new ResponseMessage<AuthResponse>()
            {
                message = "Login successfully",
                result = true,
                value = new AuthResponse()
                {
                    id = user.Id,
                    email = user.Email,
                    role = user.Role,
                    status = user.Status,
                    accessToken = accessToken,
                    refreshToken = refreshToken,
                    expireAt = expireAt
                }
            };
        }

        private (string token, DateTime expireAt) GenerateAccessToken(User user)
        {
            var jwtSection = _config.GetSection("Jwt");

            var claims = new[]
            {
                new Claim(ClaimTypes.SerialNumber, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expireHours = double.Parse(jwtSection["ExpireHours"] ?? "2");
            var expireAt = DateTime.UtcNow.AddHours(expireHours);

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expireAt,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, expireAt);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<ResponseMessage<AuthResponse>> Register(RegisterRequest requestAuthDTO)
        {
            var existingUser = await _userRepository
                .GetFirstOrDefaultAsync(u => u.Email.ToLower() == requestAuthDTO.email.ToLower());

            if (existingUser != null)
                throw new NotFoundException("Email existed");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(requestAuthDTO.password);

            var newUser = new User
            {
                Email = requestAuthDTO.email,
                HashPassword = hashedPassword,
                Role = requestAuthDTO.role
            };

            _userRepository.Add(newUser);
            await _userRepository.SaveChangesAsync();

            var (token, expireAt) = GenerateAccessToken(newUser);

            return new ResponseMessage<AuthResponse>()
            {
                message = "Register successfully",
                result = true,
                value = new AuthResponse
                {
                    id = newUser.Id,
                    email = requestAuthDTO.email,
                    role = newUser.Role,
                    status = newUser.Status,
                    accessToken = token,
                    expireAt = expireAt
                }
            };
        }

        public async Task<ResponseMessage<AuthResponse>> GenerateRefreshToken(TokenRequest tokenRequest)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.RefreshToken == tokenRequest.refreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnauthorizedException("Invalid or expired refresh token. Please login again");

            var (newAccessToken, expireAt) = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return new ResponseMessage<AuthResponse>()
            {
                message = "Renewed refresh token",
                result = true,
                value = new AuthResponse()
                {
                    id = user.Id,
                    email = user.Email,
                    role = user.Role,
                    status = user.Status,
                    accessToken = newAccessToken,
                    refreshToken = newRefreshToken,
                    expireAt = expireAt
                }
            };
        }
    }
}

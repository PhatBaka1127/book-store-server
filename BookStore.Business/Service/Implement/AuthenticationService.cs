using AutoMapper;
using BookStore.Business.Dto;
using BookStore.Business.Helper;
using BookStore.Business.Service.Implement;
using BookStore.Business.Service.Interface;
using BookStore.Data.Entity;
using BookStore.Data.Helper;
using BookStore.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

            string accessToken = GenerateAccessToken(user);
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
                    accessToken = accessToken,
                    email = user.Email,
                    id = user.Id,
                    role = user.Role,
                    status = user.Status,
                    refreshToken = refreshToken
                }
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private string GenerateAccessToken(User user)
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

            var expires = DateTime.Now.AddHours(double.Parse(jwtSection["ExpireHours"] ?? "2"));

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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

            var token = GenerateAccessToken(newUser);

            return new ResponseMessage<AuthResponse>()
            {
                message = "Register successfully",
                result = true,
                value = new AuthResponse
                {
                    email = requestAuthDTO.email,
                    accessToken = token,
                    id = newUser.Id,
                    role = newUser.Role,
                    status = newUser.Status,
                }
            };
        }

        public async Task<ResponseMessage<AuthResponse>> GenerateRefreshToken(TokenRequest tokenRequest)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.RefreshToken == tokenRequest.refreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnauthorizedException("Invalid or expired refresh token. Please login again");

            var newAccessToken = GenerateAccessToken(user);
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
                    accessToken = newAccessToken,
                    email = user.Email,
                    refreshToken = newRefreshToken,
                    role = user.Role,
                    status = user.Status
                }
            };
        }
    }
}


using AutoMapper;
using BookStore.Business.Dto;
using BookStore.Business.Service.Implement;
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
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Interface
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

        public async Task<ResponseAuthDTO> Login(LoginRequestDTO requestAuthDTO)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(
                x => x.Email.ToLower() == requestAuthDTO.email.ToLower()
            );

            if (user == null)
                throw new NotFoundException("Email không tồn tại");

            bool validPassword = BCrypt.Net.BCrypt.Verify(requestAuthDTO.password, user.HashPassword);

            if (!validPassword)
                throw new NotFoundException("Mật khẩu không đúng");

            string token = GenerateJwtToken(user);

            return new ResponseAuthDTO
            {
                accessToken = token,
                email = user.Email,
                id = user.Id,
                role = user.Role,
                status = user.Status,
            };
        }

        public string GenerateJwtToken(User user)
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

        public async Task<ResponseAuthDTO> Register(RegisterRequestDTO requestAuthDTO)
        {
            var existingUser = await _userRepository
                .GetFirstOrDefaultAsync(u => u.Email.ToLower() == requestAuthDTO.email.ToLower());

            if (existingUser != null)
                throw new NotFoundException("Email này đã tôn tại");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(requestAuthDTO.password);

            var newUser = new User
            {
                Email = requestAuthDTO.email,
                HashPassword = hashedPassword,
                Role = requestAuthDTO.role
            };

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            var token = GenerateJwtToken(newUser);

            return new ResponseAuthDTO
            {
                email = requestAuthDTO.email,
                accessToken = token,
                id = newUser.Id,
                role = newUser.Role,
                status = newUser.Status,
            };
        }
    }
}


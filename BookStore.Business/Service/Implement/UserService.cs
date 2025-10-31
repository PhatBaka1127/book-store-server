using AutoMapper;
using BookStore.Business.Dto;
using BookStore.Business.Service.Implement;
using BookStore.Business.Service.Interface;
using BookStore.Data.Entity;
using BookStore.Data.Helper;
using BookStore.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Implement
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository, 
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserResponse> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            if (user != null)
            {
                UserResponse userDTO = _mapper.Map<UserResponse>(user);
                return userDTO;
            }
            else
            {
                throw new NotFoundException($"Không tìm thấy user với email {email}");
            }
        }
    }
}

using AutoMapper;
using BookStore.Business.Dto;
using BookStore.Business.Service.Implement;
using BookStore.Data.Entity;
using BookStore.Data.Helper;
using BookStore.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Interface
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

        public async Task<GetUserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            if (user != null)
            {
                GetUserDTO userDTO = _mapper.Map<GetUserDTO>(user);
                return userDTO;
            }
            else
            {
                throw new NotFoundException($"Không tìm thấy user với email {email}");
            }
        }
    }
}

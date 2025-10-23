using BookStore.Business.Dto;
using BookStore.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Implement
{
    public interface IUserService
    {
        public Task<GetUserDTO> GetUserByEmailAsync(string email);
    }
}

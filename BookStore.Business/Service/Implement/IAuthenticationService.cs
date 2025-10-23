using BookStore.Business.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Implement
{
    public interface IAuthenticationService
    {
        public Task<ResponseAuthDTO> Login(LoginRequestDTO requestAuthDTO);
        public Task<ResponseAuthDTO> Register(RegisterRequestDTO requestAuthDTO);
    }
}

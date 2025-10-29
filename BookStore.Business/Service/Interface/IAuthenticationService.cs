using BookStore.Business.Dto;
using BookStore.Business.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Interface
{
    public interface IAuthenticationService
    {
        public Task<ResponseMessage<AuthDTO>> Login(LoginRequestDTO requestAuthDTO);
        public Task<ResponseMessage<AuthDTO>> Register(RegisterRequestDTO requestAuthDTO);
    }
}

using BookStore.Business.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Implement
{
    public interface IAuthenticationService
    {
        public Task<ResponseAuthDTO> Login(RequestAuthDTO requestAuthDTO);
        public Task<ResponseAuthDTO> Register(RequestAuthDTO requestAuthDTO);
    }
}

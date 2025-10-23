using BookStore.Business.Dto;
using BookStore.Business.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Implement
{
    public interface IBookService
    {
        public Task<ResponseMessage<int>> CreateBookAsync(CreateBookDTO createBookDTO, ThisUserObj thisUserObj);
    }
}

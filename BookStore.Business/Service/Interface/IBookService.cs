using BookStore.Business.Dto;
using BookStore.Business.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Interface
{
    public interface IBookService
    {
        public Task<ResponseMessage<int>> CreateBookAsync(CreateBookDTO createBookDTO, ThisUserObj thisUserObj);
        public Task<DynamicResponseModel<GetBookDTO>> GetBooksAsync(PagingRequest pagingRequest, BookFilter bookFilter, ThisUserObj thisUserObj);
        public Task<ResponseMessage<GetBookDTO>> GetBookByIdAsync(int id);
        public Task<ResponseMessage<bool>> UpdateBookAsync(int id, UpdateBookDTO updateBookDTO, ThisUserObj thisUserObj);
    }
}

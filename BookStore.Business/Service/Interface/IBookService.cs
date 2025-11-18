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
        public Task<ResponseMessage<int>> CreateBookAsync(CreateBookRequest createBookDTO, ThisUserObj thisUserObj);
        public Task<DynamicResponseModel<GetBookResponse>> GetBooksAsync(PagingRequest pagingRequest, BookFilter bookFilter, ThisUserObj thisUserObj);
        public Task<ResponseMessage<GetBookResponse>> GetBookByIdAsync(int id);
        public Task<ResponseMessage<bool>> UpdateBookAsync(int id, UpdateBookRequest updateBookDTO, ThisUserObj thisUserObj);
        public Task<ResponseMessage<bool>> DeleteBookAsync(int id, ThisUserObj thisUserObj);
    }
}

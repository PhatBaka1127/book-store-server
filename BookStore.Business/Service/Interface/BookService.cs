using AutoMapper;
using BookStore.Business.Dto;
using BookStore.Business.Helper;
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
    public class BookService : IBookService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IRepository<Book> bookRepository, 
            IRepository<Category> categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<ResponseMessage<int>> CreateBookAsync(CreateBookDTO createBookDTO, ThisUserObj thisUserObj)
        {
            var existedCategory = await _categoryRepository.FindAsync(createBookDTO.categoryId);
            if (existedCategory == null)
                throw new NotFoundException("Category not found");

            Book newBook = _mapper.Map<Book>(createBookDTO);
            newBook.SellerId = thisUserObj.userId;

            await _bookRepository.AddAsync(newBook);
            await _bookRepository.SaveChangesAsync();

            return new ResponseMessage<int>()
            {
                message = "Create new book successfully",
                result = true,
                value = newBook.Id
            };
        }
    }
}

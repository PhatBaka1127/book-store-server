using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Business.Dto;
using BookStore.Business.Helper;
using BookStore.Business.Service.Implement;
using BookStore.Business.Service.Interface;
using BookStore.Data.Entity;
using BookStore.Data.Helper;
using BookStore.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Implement
{
    public class BookService : IBookService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IRepository<Book> bookRepository,
            IRepository<Category> categoryRepository,
            ICloudinaryService cloudinaryService,
            IMapper mapper)
        {
            _cloudinaryService = cloudinaryService;
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<ResponseMessage<int>> CreateBookAsync(CreateBookRequest createBookDTO, ThisUserObj thisUserObj)
        {
            var existedCategory = await _categoryRepository.FindAsync(createBookDTO.categoryId);
            if (existedCategory == null)
                throw new NotFoundException("Category not found");

            Book newBook = _mapper.Map<Book>(createBookDTO);
            newBook.SellerId = thisUserObj.userId;

            if (createBookDTO.image != null && createBookDTO.image.Length > 0)
                newBook.Image = await _cloudinaryService.UploadImageAsync(createBookDTO.image);

            _bookRepository.Add(newBook);
            await _bookRepository.SaveChangesAsync();

            return new ResponseMessage<int>()
            {
                message = "Create new book successfully",
                result = true,
                value = newBook.Id
            };
        }

        public async Task<ResponseMessage<bool>> DeleteBookAsync(int id, ThisUserObj thisUserObj)
        {
            var existedBook = await _bookRepository.GetByIdAsync(id, includeProperties: x => x.Include(x => x.OrderDetails));
            if (existedBook == null)
                throw new NotFoundException("Book not found");
            if (existedBook.SellerId != thisUserObj.userId)
                throw new ForbiddenException("Fobidden");

            if (existedBook.OrderDetails.Count > 0)
            {
                _bookRepository.Update(existedBook);
            }
            else
                _bookRepository.Delete(existedBook);

            await _bookRepository.SaveChangesAsync();
            return new ResponseMessage<bool>()
            {
                message = "Delete successfully",
                result = true,
                value = true
            };
        }

        public async Task<ResponseMessage<GetBookResponse>> GetBookByIdAsync(int id)
        {
            var existedBook = await _bookRepository.GetByIdAsync(id, includeProperties: x => x.Include(x => x.Category)
                                                                                                .Include(x => x.Seller));
            if (existedBook == null)
                throw new NotFoundException("Book not found");
            return new ResponseMessage<GetBookResponse>()
            {
                message = "Book found",
                result = true,
                value = _mapper.Map<GetBookResponse>(existedBook)
            };
        }

        public async Task<DynamicResponseModel<GetBookResponse>> GetBooksAsync(
            PagingRequest paging, BookFilter filter, ThisUserObj user)
        {
            var query = _bookRepository.GetTable()
                .ProjectTo<GetBookResponse>(_mapper.ConfigurationProvider)
                .Where(b => string.IsNullOrEmpty(filter.name) || b.name.Contains(filter.name))
                .Where(b => !filter.categoryId.HasValue || b.categoryId == filter.categoryId.Value);

            if (user != null && user.role == 1) // SELLER
                query = query.Where(b => b.sellerId == user.userId);

            var (total, data) = query.PagingIQueryable(
                paging.page, paging.pageSize, PageConstant.LIMIT_PAGING, PageConstant.DEFAULT_PAPING);

            return new DynamicResponseModel<GetBookResponse>
            {
                metaData = new MetaData
                {
                    page = paging.page,
                    size = paging.pageSize,
                    total = total
                },
                results = await data.ToListAsync()
            };
        }

        public async Task<ResponseMessage<bool>> UpdateBookAsync(int id, UpdateBookRequest updateBookDTO, ThisUserObj thisUserObj)
        {
            var existedBook = await _bookRepository.GetByIdAsync(id, isTracking: true);
            if (existedBook == null)
                throw new NotFoundException("Not found this book");
            if (thisUserObj.userId != existedBook.SellerId)
                throw new ForbiddenException("Forbidden");

            _mapper.Map(updateBookDTO, existedBook);

            if (updateBookDTO.image != null && updateBookDTO.image.Length > 0)
                existedBook.Image = await _cloudinaryService.UploadImageAsync(updateBookDTO.image);

            _bookRepository.Update(existedBook);
            await _bookRepository.SaveChangesAsync();

            return new ResponseMessage<bool>
            {
                message = "Update successfully",
                result = true,
                value = true
            };
        }
    }
}

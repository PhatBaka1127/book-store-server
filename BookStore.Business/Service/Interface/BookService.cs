using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Business.Dto;
using BookStore.Business.Helper;
using BookStore.Business.Service.Implement;
using BookStore.Data.Entity;
using BookStore.Data.Helper;
using BookStore.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Interface
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

        public async Task<ResponseMessage<int>> CreateBookAsync(CreateBookDTO createBookDTO, ThisUserObj thisUserObj)
        {
            var existedCategory = await _categoryRepository.FindAsync(createBookDTO.categoryId);
            if (existedCategory == null)
                throw new NotFoundException("Category not found");

            Book newBook = _mapper.Map<Book>(createBookDTO);
            newBook.SellerId = thisUserObj.userId;

            if (createBookDTO.image != null && createBookDTO.image.Length > 0)
                newBook.Image = await _cloudinaryService.UploadImageAsync(createBookDTO.image);

            await _bookRepository.AddAsync(newBook);
            await _bookRepository.SaveChangesAsync();

            return new ResponseMessage<int>()
            {
                message = "Create new book successfully",
                result = true,
                value = newBook.Id
            };
        }

        public async Task<ResponseMessage<GetBookDTO>> GetBookByIdAsync(int id)
        {
            var existedBook = await _bookRepository.GetByIdAsync(id, includeProperties: x => x.Include(x => x.Category)
                                                                                                .Include(x => x.Seller));
            if (existedBook == null)
                throw new NotFoundException("Book not found");
            return new ResponseMessage<GetBookDTO>()
            {
                message = "Book found",
                result = true,
                value = _mapper.Map<GetBookDTO>(existedBook)
            };
        }

        public async Task<DynamicResponseModel<GetBookDTO>> GetBooksAsync(PagingRequest pagingRequest, BookFilter bookFilter)
        {
            (int, IQueryable<GetBookDTO>) result;

            var query = _bookRepository.GetTable()
                            .ProjectTo<GetBookDTO>(_mapper.ConfigurationProvider)
                            .Where(x => bookFilter.name == null || x.name.Contains(bookFilter.name));

            result = query.PagingIQueryable(pagingRequest.page, pagingRequest.pageSize, PageConstant.LIMIT_PAGING, PageConstant.DEFAULT_PAPING);

            return new DynamicResponseModel<GetBookDTO>()
            {
                metaData = new MetaData()
                {
                    page = pagingRequest.page,
                    size = pagingRequest.pageSize,
                    total = result.Item1
                },
                results = await result.Item2.ToListAsync()
            };
        }
    }
}

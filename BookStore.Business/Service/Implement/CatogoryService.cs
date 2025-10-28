using AutoMapper;
using BookStore.Business.Dto;
using BookStore.Business.Service.Interface;
using BookStore.Data.Entity;
using BookStore.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Business.Service.Implement
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IRepository<Category> categoryRepository,
                                IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<GetCategoryDTO>> GetCategoriesAsync()
        {
            return _mapper.Map<List<GetCategoryDTO>>(await _categoryRepository.GetTable().ToListAsync());
        }
    }
}
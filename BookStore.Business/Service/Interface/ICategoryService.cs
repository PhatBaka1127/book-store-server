using BookStore.Business.Dto;

namespace BookStore.Business.Service.Interface
{
    public interface ICategoryService
    {
        public Task<List<GetCategoryDTO>> GetCategoriesAsync();
    }
}

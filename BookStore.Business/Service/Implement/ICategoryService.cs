using BookStore.Business.Dto;

public interface ICategoryService
{
    public Task<List<GetCategoryDTO>> GetCategoriesAsync();
}
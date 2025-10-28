using Microsoft.AspNetCore.Http;

namespace BookStore.Business.Service.Interface
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
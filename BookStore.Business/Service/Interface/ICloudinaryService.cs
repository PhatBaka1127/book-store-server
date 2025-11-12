using Microsoft.AspNetCore.Http;

namespace BookStore.Business.Service.Interface
{
    public interface ICloudinaryService
    {
        public Task<string> UploadImageAsync(IFormFile file);
    }
}
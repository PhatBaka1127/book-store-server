using Microsoft.AspNetCore.Http;

namespace BookStore.Business.Dto
{
    public class ShopDTO
    {
        public string? address { get; set; }
        public decimal? lon { get; set; }
        public decimal? lat { get; set; }
    }

    public class CreateShopRequest
    {
        public IFormFile? image { get; set; }
        public DateTime createdDate = DateTime.UtcNow;
    }

    public class UpdateShopRequest
    {
        public IFormFile? image { get; set; }
    }

    public class ShopResponse
    {

    }
    
    public class ShopFilter
    {
        
    }
}
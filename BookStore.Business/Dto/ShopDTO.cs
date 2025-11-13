using Microsoft.AspNetCore.Http;

namespace BookStore.Business.Dto
{
    public class ShopDTO
    {
        public string? address { get; set; }
        public decimal? lon { get; set; }
        public decimal? lat { get; set; }
    }

    public class CreateShopRequest : ShopDTO
    {
        public IFormFile? image { get; set; }
        public DateTime createdDate = DateTime.UtcNow;
    }

    public class UpdateShopRequest : ShopDTO
    {
        public IFormFile? image { get; set; }
        public DateTime updatedDate = DateTime.UtcNow;
    }

    public class ShopResponse : ShopDTO
    {
        public int? id { get; set; }
        public string? image { get; set; }
        public DateTime? createdDate { get; set; }
    }
    
    public class ShopFilter
    {
        public DateTime? startDate { get; set; }
        public DateTime? enddate { get; set; }
    }
}
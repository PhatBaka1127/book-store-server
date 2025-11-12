using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data.Entity
{
    [Table(nameof(Book))]
    public partial class Book : IdentityEntity
    {
        public Book()
        {
            OrderDetails = [];
            Ratings = [];
            Carts = [];
        }

        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; } = 0;
        public int Stock { get; set; } = 0;
        public string? Image { get; set; }
        public double AvageStar => Ratings.Average(x => x.Star);

        public int SellerId { get; set; }
        [ForeignKey(nameof(SellerId))]
        [InverseProperty(nameof(User.Books))]
        public required User Seller { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        [InverseProperty(nameof(Category.Books))]
        public required Category Category { get; set; }
        
        [InverseProperty(nameof(OrderDetail.Book))]
        public ICollection<OrderDetail> OrderDetails { get; set; }
        [InverseProperty(nameof(Rating.Book))]
        public ICollection<Rating> Ratings { get; set; }
        [InverseProperty(nameof(Cart.Book))]
        public ICollection<Cart> Carts { get; set; }
    }
}

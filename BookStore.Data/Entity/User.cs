using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data.Entity
{
    [Table(nameof(User))]
    public class User : IdentityEntity
    {
        public User()
        {
            Books = [];
            Orders = [];
            Shops = [];
            Carts = [];
        }

        public required string Email { get; set; }
        public string? HashPassword { get; set; }
        public string? Image { get; set; }
        // 0: Buyer
        // 1: Seller
        // 2: Shipper
        // 3: Admin
        public int Role { get; set; } = 0;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        [InverseProperty(nameof(Order.Buyer))]
        public ICollection<Order> Orders { get; set; }
        [InverseProperty(nameof(Book.Seller))]
        public ICollection<Book> Books { get; set; }
        [InverseProperty(nameof(Shop.Users))]
        public ICollection<Shop> Shops { get; set; }
        [InverseProperty(nameof(Cart.User))]
        public ICollection<Cart> Carts { get; set; }
    }
}

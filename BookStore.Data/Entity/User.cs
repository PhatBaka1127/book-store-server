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
    public class User
    {
        public User()
        {
            Books = [];
            Orders = [];
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public required string Email { get; set; }
        public string? HashPassword { get; set; }
        public string? Image { get; set; }
        // 0: Buyer
        // 1: Seller
        public int Role { get; set; } = 0;
        public int Status { get; set; } = 1;

        [InverseProperty(nameof(Order.Buyer))]
        public virtual ICollection<Order> Orders { get; set; }

        [InverseProperty(nameof(Book.Seller))]
        public virtual ICollection<Book> Books { get; set; }
    }
}

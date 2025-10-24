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
    public partial class Book
    {
        public Book()
        {
            OrderDetails = [];
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; } = 0;
        public int Stock { get; set; } = 0;
        public int Status { get; set; } = 1;
        public string? Image { get; set; }

        public int SellerId { get; set; }
        [ForeignKey(nameof(SellerId))]
        [InverseProperty(nameof(User.Books))]
        public required virtual User Seller { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        [InverseProperty(nameof(Category.Books))]
        public required virtual Category Category { get; set; }

        [InverseProperty(nameof(OrderDetail.Book))]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

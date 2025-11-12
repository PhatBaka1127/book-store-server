using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Data.Entity
{
    [Table(nameof(Cart))]
    public class Cart
    {
        [Key]
        public int BookId { get; set; }
        [ForeignKey(nameof(BookId))]
        [InverseProperty(nameof(Book.Carts))]
        public Book? Book { get; set; }

        [Key]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(User.Carts))]
        public User? User { get; set; }

        public int Quantity { get; set; }
    }
}
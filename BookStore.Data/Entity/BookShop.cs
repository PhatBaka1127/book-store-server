using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Data.Entity
{
    [Table(nameof(BookShop))]
    public partial class BookShop
    {
        [Key]
        public int BookId { get; set; }
        [ForeignKey(nameof(BookId))]
        [InverseProperty(nameof(Book.BookShops))]
        public Book? Book { get; set; }

        [Key]
        public int ShopId { get; set; }
        [ForeignKey(nameof(ShopId))]
        [InverseProperty(nameof(Shop.BookShops))]
        public Shop? Shop { get; set; }

        public int Stock { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data.Entity
{
    [Table(nameof(Category))]
    public partial class Category
    {
        public Category()
        {
            Books = [];
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public required string Name { get; set; }
        public int Status { get; set; } = 1;

        [InverseProperty(nameof(Book.Category))]
        public virtual ICollection<Book> Books { get; set; }
    }
}

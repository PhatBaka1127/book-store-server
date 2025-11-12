using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data.Entity
{
    [Table(nameof(Rating))]
    public class Rating : EntityBase
    {
        [Key]
        public int? OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(Order.Ratings))]
        public virtual Order? Order { get; set; }
        [Key]
        public int? BookId { get; set; }
        [ForeignKey(nameof(BookId))]
        [InverseProperty(nameof(Book.Ratings))]
        public virtual Book? Book { get; set; }

        public int Star { get; set; }
        public string? Comment { get; set; }
    }
}
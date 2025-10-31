using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data.Entity
{
    [Table(nameof(OrderDetail))]
    public partial class OrderDetail
    {
        [Key]
        public int? OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(Order.OrderDetails))]
        public virtual Order? Order { get; set; }
        [Key]
        public int? BookId { get; set; }
        [ForeignKey(nameof(BookId))]
        [InverseProperty(nameof(Book.OrderDetails))]
        public virtual Book? Book { get; set; }

        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}

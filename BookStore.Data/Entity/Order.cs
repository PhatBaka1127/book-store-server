using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data.Entity
{
    [Table(nameof(Order))]
    public partial class Order
    {
        public Order()
        {
            OrderDetails = [];
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public int BuyerId { get; set; }
        [ForeignKey(nameof(BuyerId))]
        [InverseProperty(nameof(User.Orders))]
        public virtual User Buyer { get; set; }

        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int Status { get; set; }

        [InverseProperty(nameof(OrderDetail.Order))]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

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
    public partial class Order : IdentityEntity
    {
        public Order()
        {
            OrderDetails = [];
            Ratings = [];
        }

        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }

        public int BuyerId { get; set; }
        [ForeignKey(nameof(BuyerId))]
        [InverseProperty(nameof(User.Orders))]
        public required User Buyer { get; set; }

        [InverseProperty(nameof(OrderDetail.Order))]
        public ICollection<OrderDetail> OrderDetails { get; set; }
        [InverseProperty(nameof(Rating.Order))]
        public ICollection<Rating> Ratings { get; set; } 
    }
}

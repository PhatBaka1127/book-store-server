using BookStore.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Dto
{
    public class GetOrderDetailDTO
    {
        public int? orderId { get; set; }
        public int? bookId { get; set; }
        public decimal? unitPrice { get; set; }
        public int? quantity { get; set; }
        public decimal? totalPrice { get; set; }
        public string? bookName { get; set; }
        public string? bookImage { get; set; }
    }

    public class CreateOrderDetailDTO
    {
        public int? bookId { get; set; }
        public int? quantity { get; set; }
    }
}

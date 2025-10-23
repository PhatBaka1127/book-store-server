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
    public class GetOrderDTO
    {
        public int? id { get; set; }
        public int? userId { get; set; }
        public string? userName { get; set; }
        public string? createdDate { get; set; }
        public int? quantity { get; set; }
        public decimal? totalPrice { get; set; }
        public ICollection<GetOrderDetailDTO>? orderDetails { get; set; }
    }
}

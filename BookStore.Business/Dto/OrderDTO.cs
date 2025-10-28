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
        public string? createdDate { get; set; }
        public int? quantity { get; set; }
        public decimal? totalPrice { get; set; }
        public OrderStatus status { get; set; }
        public string? address { get; set; }
        public string? phone { get; set; }
    }

    public class GetDetailOrderDTO : GetOrderDTO
    {
        public GetOrderDetailDTO[] orderDetails { get; set; }
    }

    public class CreateOrderDTO
    {
        public string phone { get; set; }
        public string address { get; set; }
        public int status = (int) OrderStatus.ORDERED;
        public int quantity => (int) createOrderDetailDTOs.Sum(x => x.quantity);
        public CreateOrderDetailDTO[] createOrderDetailDTOs { get; set; }
    }

    public class OrderFilter
    {
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
        public OrderStatus status { get; set; }
    }

    public enum OrderStatus
    {
        ORDERED = 0,
        DELIVERING = 1,
        DELIVERED = 2,
        FAILED = 3
    }
}
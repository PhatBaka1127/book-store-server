using BookStore.Business.Helper;
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
    public class OrderResponse
    {
        public int? id { get; set; }
        public string? createdDate { get; set; }
        public string? updatedDate { get; set; } = "N/A";
        public int? quantity { get; set; }
        public decimal? totalPrice { get; set; }
        public int? status { get; set; }
        public string? address { get; set; }
        public string? phone { get; set; }
        public string? buyerId { get; set; }
        public string? buyerEmail { get; set; }
    }

    public class DetailOrderResponse : OrderResponse
    {
        public OrderDetailResponse[] orderDetails { get; set; }
    }

    public class CreateOrderRequest
    {
        public string phone { get; set; }
        public string address { get; set; }
        public int status = (int)OrderStatus.ORDERED;
        public int quantity => (int)createOrderDetailDTOs.Sum(x => x.quantity);
        public CreateOrderDetailRequest[] createOrderDetailDTOs { get; set; }
    }

    public class OrderFilter
    {
        private DateTime? _startTime;
        private DateTime? _endTime;

        public DateTime? startTime
        {
            get => _startTime;
            set => _startTime = value.HasValue
                ? DateTime.SpecifyKind(value.Value.Date, DateTimeKind.Utc) // Bắt đầu từ đầu ngày UTC
                : null;
        }

        public DateTime? endTime
        {
            get => _endTime;
            set => _endTime = value.HasValue
                ? DateTime.SpecifyKind(
                    value.Value.Date.AddDays(1).AddTicks(-1), // Cuối ngày: 23:59:59.9999999
                    DateTimeKind.Utc)
                : null;
        }

        public OrderStatus? status { get; set; }
        [Sort]
        public string? sort { get; set; }
    }

    public enum OrderStatus
    {
        ORDERED = 0,
        DELIVERING = 1,
        DELIVERED = 2,
        FAILED = 3
    }

    public class ReportFilter
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public ReportFilterEnum reportFilterEnum { get; set; }
    }

    public class DashboardSummaryResponse
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<DailySummaryResponse> DailySummaries { get; set; } = new();
    }

    public class DailySummaryResponse
    {
        public DateTime Date { get; set; }
        public int Orders { get; set; }
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }

    public enum ReportFilterEnum
    {
        DAY = 0,
        MONTH = 1,
        YEAR = 2
    }
}
using BookStore.Business.Dto;
using BookStore.Business.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Interface
{
    public interface IOrderService
    {
        // CREATE
        public Task<ResponseMessage<int>> CreateOrderAsync(CreateOrderRequest createOrderDTO, ThisUserObj thisUserObj);
        
        // READ
        public Task<ResponseMessage<DetailOrderResponse>> GetOrderById(int id, ThisUserObj thisUserObj);
        public Task<DynamicResponseModel<OrderResponse>> GetOrders(ThisUserObj thisUserObj, PagingRequest pagingRequest, OrderFilter orderFilter);
        public Task<List<DailySummaryResponse>> GetOrderReport(ThisUserObj thisUserObj, ReportFilter reportFilter);
    }
}
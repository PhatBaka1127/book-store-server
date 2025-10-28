using BookStore.Business.Dto;
using BookStore.Business.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Implement
{
    public interface IOrderService
    {
        public Task<ResponseMessage<int>> CreateOrderAsync(CreateOrderDTO createOrderDTO, ThisUserObj thisUserObj);
        public Task<ResponseMessage<GetDetailOrderDTO>> GetOrderById(int id);
        public Task<DynamicResponseModel<GetOrderDTO>> GetOrders(ThisUserObj thisUserObj, PagingRequest pagingRequest, OrderFilter orderFilter);
    }
}

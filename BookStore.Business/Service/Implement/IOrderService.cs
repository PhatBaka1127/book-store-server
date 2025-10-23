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
        public Task<ResponseMessage<int>> CreateOrderAsync(CreateOrderDetailDTO[] createOrderDetailDTOs, ThisUserObj thisUserObj);
        public Task<ResponseMessage<GetOrderDTO>> GetOrderById(int id);
    }
}

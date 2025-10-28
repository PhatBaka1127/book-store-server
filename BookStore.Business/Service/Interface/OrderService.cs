using AutoMapper;
using BookStore.Business.Dto;
using BookStore.Business.Helper;
using BookStore.Business.Service.Implement;
using BookStore.Data.Entity;
using BookStore.Data.Helper;
using BookStore.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Interface
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IRepository<Order> orderRepository,
            IRepository<Book> bookRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<ResponseMessage<int>> CreateOrderAsync(CreateOrderDTO createOrderDTO, ThisUserObj thisUserObj)
        {
            Order newOrder = _mapper.Map<Order>(createOrderDTO);
            
            foreach (var createOrderDetailDTO in createOrderDTO.createOrderDetailDTOs)
            {
                var existedBook = await _bookRepository.FindAsync(createOrderDetailDTO.bookId);
                if (existedBook == null)
                    throw new NotFoundException($"Not found book {createOrderDetailDTO.bookId}");
                OrderDetail orderDetail = new()
                {
                    BookId = existedBook.Id,
                    Quantity = (int) createOrderDetailDTO.quantity,
                    UnitPrice = existedBook.UnitPrice
                };
                newOrder.OrderDetails.Add(orderDetail);
            }

            newOrder.CreatedDate = DateTime.UtcNow;
            newOrder.BuyerId = thisUserObj.userId;

            await _orderRepository.AddAsync(newOrder);
            await _orderRepository.SaveChangesAsync();

            return new ResponseMessage<int>()
            {
                message = "Create order successfully",
                result = true,
                value = newOrder.Id
            };
        }

        public async Task<ResponseMessage<GetOrderDTO>> GetOrderById(int id)
        {
            var existedOrder = await _orderRepository.GetByIdAsync(id, includeProperties: x => x.Include(x => x.OrderDetails)
                                                                                                    .ThenInclude(x => x.Book)
                                                                                                .Include(x => x.Buyer));
            if (existedOrder == null)
                throw new NotFoundException("Order not found");
            return new ResponseMessage<GetOrderDTO>()
            {
                message = "Order found",
                result = true,
                value = _mapper.Map<GetOrderDTO>(existedOrder)
            };
        }
    }
}

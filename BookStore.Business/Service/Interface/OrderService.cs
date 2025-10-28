using AutoMapper;
using AutoMapper.QueryableExtensions;
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
                    Quantity = (int)createOrderDetailDTO.quantity,
                    UnitPrice = existedBook.UnitPrice
                };
                newOrder.OrderDetails.Add(orderDetail);
            }

            newOrder.TotalPrice = newOrder.OrderDetails.Sum(x => x.TotalPrice);
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

        public async Task<ResponseMessage<GetDetailOrderDTO>> GetOrderById(int id)
        {
            var existedOrder = await _orderRepository.GetByIdAsync(id, includeProperties: x => x.Include(x => x.OrderDetails)
                                                                                                    .ThenInclude(x => x.Book)
                                                                                                .Include(x => x.Buyer));
            if (existedOrder == null)
                throw new NotFoundException("Order not found");
            return new ResponseMessage<GetDetailOrderDTO>()
            {
                message = "Order found",
                result = true,
                value = _mapper.Map<GetDetailOrderDTO>(existedOrder)
            };
        }

        public async Task<DynamicResponseModel<GetOrderDTO>> GetOrders(
            ThisUserObj user, PagingRequest paging, OrderFilter filter)
        {
            var (total, data) = _orderRepository.GetTable()
                .Where(o => user.role != (int)RoleEnum.BUYER || o.BuyerId == user.userId)
                .ProjectTo<GetOrderDTO>(_mapper.ConfigurationProvider)
                .PagingIQueryable(paging.page, paging.pageSize,
                                  PageConstant.LIMIT_PAGING, PageConstant.DEFAULT_PAPING);

            return new DynamicResponseModel<GetOrderDTO>
            {
                metaData = new MetaData
                {
                    page = paging.page,
                    size = paging.pageSize,
                    total = total
                },
                results = await data.ToListAsync()
            };
        }
    }
}
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Business.Dto;
using BookStore.Business.Helper;
using BookStore.Business.Service.Implement;
using BookStore.Business.Service.Interface;
using BookStore.Data.Entity;
using BookStore.Data.Helper;
using BookStore.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Service.Implement
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

        public async Task<ResponseMessage<int>> CreateOrderAsync(CreateOrderRequest createOrderDTO, ThisUserObj thisUserObj)
        {
            int orderId = 0;

            await _bookRepository.ExecuteInTransactionAsync(async () =>
            {
                Order newOrder = _mapper.Map<Order>(createOrderDTO);
                newOrder.BuyerId = thisUserObj.userId;
                newOrder.CreatedDate = DateTime.UtcNow;

                foreach (var item in createOrderDTO.createOrderDetailDTOs)
                {
                    var book = await _bookRepository.FindAsync(item.bookId);
                    if (book == null)
                        throw new NotFoundException($"Book {item.bookId} not found");

                    if (book.Stock < item.quantity)
                        throw new ConflictException($"Book {book.Name} is out of stock");

                    book.Stock -= (int)item.quantity;
                    _bookRepository.Update(book);

                    newOrder.OrderDetails.Add(new OrderDetail
                    {
                        BookId = book.Id,
                        Quantity = (int)item.quantity,
                        UnitPrice = book.UnitPrice
                    });
                }

                newOrder.TotalPrice = newOrder.OrderDetails.Sum(x => x.TotalPrice);

                _orderRepository.Add(newOrder);

                await _bookRepository.SaveChangesAsync();
                await _orderRepository.SaveChangesAsync();

                orderId = newOrder.Id;
            });

            return new ResponseMessage<int>
            {
                message = "Create order successfully",
                result = true,
                value = orderId
            };
        }

        public async Task<ResponseMessage<DetailOrderResponse>> GetOrderById(int id, ThisUserObj thisUserObj)
        {
            var existedOrder = await _orderRepository.GetByIdAsync(id, includeProperties: x => x.Include(x => x.OrderDetails)
                                                                                                    .ThenInclude(x => x.Book)
                                                                                                .Include(x => x.Buyer));
            if (existedOrder == null)
                throw new NotFoundException("Order not found");
            if (thisUserObj.role == 0 && thisUserObj.userId != existedOrder.BuyerId)
                throw new ForbiddenException("Forbidden");

            existedOrder.OrderDetails = existedOrder.OrderDetails.Where(x => x.Book.SellerId == thisUserObj.userId).ToList();
            existedOrder.Quantity = existedOrder.OrderDetails.Sum(x => x.Quantity);
            existedOrder.TotalPrice = existedOrder.OrderDetails.Sum(x => x.TotalPrice);

            return new ResponseMessage<DetailOrderResponse>()
            {
                message = "Order found",
                result = true,
                value = _mapper.Map<DetailOrderResponse>(existedOrder)
            };
        }

        public async Task<List<DailySummaryResponse>> GetOrderReport(ThisUserObj thisUserObj, ReportFilter reportFilter)
        {
            var query = _orderRepository.GetTable()
                            .Where(o => o.OrderDetails.Any(od => od.Book.SellerId == thisUserObj.userId));

            if (reportFilter.startDate.HasValue)
                query = query.Where(o => o.CreatedDate.ToUniversalTime() >= reportFilter.startDate.Value.ToUniversalTime());
            if (reportFilter.endDate.HasValue)
                query = query.Where(o => o.CreatedDate.ToUniversalTime() <= reportFilter.endDate.Value.ToUniversalTime());

            var groupedQuery = reportFilter.reportFilterEnum switch
            {
                ReportFilterEnum.MONTH => query.GroupBy(o => new { o.CreatedDate.Year, o.CreatedDate.Month })
                                .Select(g => new DailySummaryResponse
                                {
                                    Date = DateTime.SpecifyKind(new DateTime(g.Key.Year, g.Key.Month, 1), DateTimeKind.Utc),
                                    Orders = g.Count(),
                                    Quantity = g.Sum(x => x.OrderDetails.Sum(x => x.Quantity)),
                                    Revenue = g.Sum(o => o.TotalPrice)
                                }),
                ReportFilterEnum.YEAR => query.GroupBy(o => o.CreatedDate.Year)
                               .Select(g => new DailySummaryResponse
                               {
                                   Date = new DateTime(g.Key, 1, 1),
                                   Orders = g.Count(),
                                   Quantity = g.Sum(x => x.OrderDetails.Sum(x => x.Quantity)),
                                   Revenue = g.Sum(o => o.TotalPrice)
                               }),
                _ => query.GroupBy(o => o.CreatedDate.Date)
                         .Select(g => new DailySummaryResponse
                         {
                             Date = g.Key,
                             Orders = g.Count(),
                             Revenue = g.Sum(o => o.TotalPrice),
                             Quantity = g.Sum(x => x.OrderDetails.Sum(x => x.Quantity))
                         })
            };

            var result = await groupedQuery.OrderBy(d => d.Date).ToListAsync();
            return result;
        }

        public async Task<DynamicResponseModel<OrderResponse>> GetOrders(
            ThisUserObj user, PagingRequest paging, OrderFilter filter)
        {
            var (total, data) = _orderRepository.GetTable()
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Book)
                .Where(o =>
                    // BUYER
                    (user.role == (int)RoleEnum.BUYER && o.BuyerId == user.userId)
                    // SELLER
                    || (user.role == (int)RoleEnum.SELLER && o.OrderDetails.Any(od => od.Book.SellerId == user.userId))
                )
                .DynamicFilter(filter)
                .ProjectTo<OrderResponse>(_mapper.ConfigurationProvider)
                .PagingIQueryable(paging.page, paging.pageSize,
                                  PageConstant.LIMIT_PAGING, PageConstant.DEFAULT_PAPING);

            return new DynamicResponseModel<OrderResponse>
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

        public async Task<ResponseMessage<bool>> UpdateOrderDetailAsync(ThisUserObj userObj, int orderId, UpdateOrderDetailRequest[] orderDetailRequests)
        {
            var existedOrder = await _orderRepository.GetByIdAsync(orderId, includeProperties: x => x.Include(x => x.OrderDetails), isTracking: true);
            if (existedOrder == null)
                throw new NotFoundException("Order not found");

            foreach (var orderDetailRequest in orderDetailRequests)
            {
                var orderDetail = existedOrder.OrderDetails.FirstOrDefault(x => x.BookId == orderDetailRequest.bookId);
                orderDetail.Status = (int)orderDetailRequest.status;
                orderDetail.UpdatedDate = DateTime.UtcNow;

                _orderRepository.Update(existedOrder);
            }

            await _orderRepository.SaveChangesAsync();

            return new ResponseMessage<bool>()
            {
                message = "Update order detail successfully",
                result = true,
                value = true
            };
        }
    }
}
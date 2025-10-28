using BookStore.API.Extension;
using BookStore.Business.Dto;
using BookStore.Business.Helper;
using BookStore.Business.Service.Implement;
using BookStore.Business.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    [EnableCors("MyAllowSpecificOrigins")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, 
            IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
        {
            ThisUserObj currentUser = await ServiceExtension.GetThisUserInfo(HttpContext, _userService);

            var result = await _orderService.CreateOrderAsync(createOrderDTO, currentUser);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderService.GetOrderById(id);
            return Ok(result);
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetOrders([FromQuery] OrderFilter orderFilter, [FromQuery] PagingRequest pagingRequest)
        {
            ThisUserObj currentUser = await ServiceExtension.GetThisUserInfo(HttpContext, _userService);

            var result = await _orderService.GetOrders(currentUser, pagingRequest, orderFilter);
            return Ok(result);
        }
    }
}

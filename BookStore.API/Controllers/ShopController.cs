using BookStore.API.Extension;
using BookStore.Business.Dto;
using BookStore.Business.Helper;
using BookStore.Business.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("api/v1/shops")]
    [EnableCors("MyAllowSpecificOrigins")]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly IUserService _userService;

        public ShopController(IShopService shopService, 
                                IUserService userService)
        {
            _shopService = shopService;
            _userService = userService;
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> CreateShop([FromForm] CreateShopRequest createShopRequest)
        {
            ThisUserObj currentUser = await ServiceExtension.GetThisUserInfo(HttpContext, _userService);

            var result = await _shopService.CreateShopAsync(createShopRequest, currentUser);
            return Ok(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetBooks([FromQuery] ShopFilter shopFilter,
                                                        [FromQuery] PagingRequest pagingRequest,
                                                        int sellerId = 0)
        {
            ThisUserObj thisUserObj = await ServiceExtension.GetThisUserInfo(HttpContext, _userService);

            var result = await _shopService.GetShopAsync(shopFilter, thisUserObj, pagingRequest);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var result = await _shopService.GetShopByIdAsync(id);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBook(int id, [FromForm] UpdateShopRequest updateShopRequest)
        {
            ThisUserObj thisUserObj = await ServiceExtension.GetThisUserInfo(HttpContext, _userService);

            var result = await _shopService.UpdateShopByIdAsync(id, thisUserObj, updateShopRequest);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBook(int id)
        {
            ThisUserObj thisUserObj = await ServiceExtension.GetThisUserInfo(HttpContext, _userService);

            var result = await _shopService.DeleteShopAsync(id, thisUserObj);
            return Ok(result);
        }
    }
}
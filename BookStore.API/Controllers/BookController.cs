using BookStore.API.Extension;
using BookStore.Business.Dto;
using BookStore.Business.Helper;
using BookStore.Business.Service.Implement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("api/v1/book")]
    [EnableCors("MyAllowSpecificOrigins")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IUserService _userService;

        public BookController(IBookService bookService, 
            IUserService userService)
        {
            _bookService = bookService;
            _userService = userService;
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDTO voucherDTO)
        {
            ThisUserObj currentUser = await ServiceExtension.GetThisUserInfo(HttpContext, _userService);

            var result = await _bookService.CreateBookAsync(voucherDTO, currentUser);
            return Ok(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetVouchers([FromQuery] BookFilter bookFilter,
                                                        [FromQuery] PagingRequest pagingRequest)
        {
            var result = await _bookService.GetBooksAsync(pagingRequest, bookFilter);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucherById(int id)
        {
            var result = await _bookService.GetBookByIdAsync(id);
            return Ok(result);
        }
    }
}

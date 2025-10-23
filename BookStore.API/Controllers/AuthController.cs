using BookStore.Business.Dto;
using BookStore.Business.Service.Implement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    [EnableCors("MyAllowSpecificOrigins")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] RequestAuthDTO requestAuthDTO)
        {
            var result = await _authService.Login(requestAuthDTO);
            return Ok(result);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RequestAuthDTO requestAuthDTO)
        {
            var result = await _authService.Register(requestAuthDTO);
            return Ok(result);
        }
    }
}

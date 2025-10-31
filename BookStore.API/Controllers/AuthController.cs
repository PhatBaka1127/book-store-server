using BookStore.Business.Dto;
using BookStore.Business.Service.Implement;
using BookStore.Business.Service.Interface;
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
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequestDTO)
        {
            var result = await _authService.Login(loginRequestDTO);
            return Ok(result);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequestDTO)
        {
            var result = await _authService.Register(registerRequestDTO);
            return Ok(result);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateRefreshToken([FromBody] TokenRequest tokenRequest)
        {
            var result = await _authService.GenerateRefreshToken(tokenRequest);
            return Ok(result);
        }
    }
}

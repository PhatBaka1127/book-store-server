using BookStore.API.Extension;
using BookStore.Business.Dto;
using BookStore.Business.Service.Implement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("api/v1/files")]
    [EnableCors("MyAllowSpecificOrigins")]
    public class FileController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IUserService _userService;

        public FileController(ICloudinaryService cloudinaryService,
                                IUserService userService)
        {
            _cloudinaryService = cloudinaryService;
            _userService = userService;
        }

        [HttpPost()]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var result = await _cloudinaryService.UploadImageAsync(file);
            return Ok(result);
        }
    }
}
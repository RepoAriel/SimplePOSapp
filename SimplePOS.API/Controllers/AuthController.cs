using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;

namespace SimplePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var result = await authService.RegisterAsync(request);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var result = await authService.LoginAsync(request);
            return Ok(result);
        }
    }
}

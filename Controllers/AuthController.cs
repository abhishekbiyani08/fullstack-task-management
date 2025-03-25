using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagementApi.Models.DTOs;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto userRegistrationDto)
        {
            try
            {
                var userToReturn = await _authService.RegisterAsync(userRegistrationDto);
                return Ok(userToReturn);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Registration failed");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            try
            {
                var userToReturn = await _authService.LoginAsync(userLoginDto);
                return Ok(userToReturn);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Login failed");
                return Unauthorized(ex.Message);
            }
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Models;
using ToDoListAPI.Services;

namespace ToDoListAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var token = await _authenticationService.Authenticate(user);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new { token });
        }
    }
}
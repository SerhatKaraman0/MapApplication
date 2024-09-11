using System.Threading.Tasks;
using MapApplication.Interfaces;
using MapApplication.Data; // Ensure you are using the correct namespace
using Microsoft.AspNetCore.Mvc;

namespace MapApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsersDb user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserEmail) || string.IsNullOrEmpty(user.UserPassword))
            {
                return BadRequest("Invalid request");
            }

            var result = await _authService.AuthenticateUser(user.UserEmail, user.UserPassword);
            return result; // Return the IActionResult directly
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return BadRequest("Authorization header is required");
            }

            // Extract token from Authorization header
            var token = authorizationHeader.Replace("Bearer ", "").Trim();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid token format");
            }

            // Call the logout service method with the token
            _authService.LogoutUser(token);

            return Ok(new { Message = "Logged out successfully" });
        }
    }
}

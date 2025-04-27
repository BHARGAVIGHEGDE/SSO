using ssobackend.Models;
using ssobackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace ssobackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("token/{userId}")]
        public async Task<IActionResult> GetTokenByUserId(int userId)
        {
            var token = await _userService.GetTokenByUserIdAsync(userId);
            return Ok(token.Token); 
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userService.AuthenticateUserAsync(loginRequest.Username, loginRequest.Password);
            var token = _userService.GenerateJwtToken(user);
            await _userService.StoreTokenInDb(user.Id, token);
            return Ok(new { token, userId = user.Id });
        }

        [HttpPost("validate")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult ValidateToken()
        {
            return Ok("Token is valid.");
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdClaim.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            return Ok(new { username = user.Username });
        }

    }
}

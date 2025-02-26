using Microsoft.AspNetCore.Mvc;
using ASP.FeedApp.API.Models;
using ASP.FeedApp.API.Services;
using System.Threading.Tasks;

namespace ASP.FeedApp.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            var user = await _userService.GetUserByUsernameAsync(userDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(new { message = "Login successful", userId = user.Id });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserRegisterDto userDto)
        {
            var existingUser = await _userService.GetUserByUsernameAsync(userDto.Username);
            if (existingUser != null)
            {
                return BadRequest(new { message = "User already exists" });
            }

            var newUser = await _userService.CreateUserAsync(userDto.Username, userDto.Password);
            return Ok(new { message = "User created successfully", userId = newUser.Id });
        }
    }
    public class UserLoginDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class UserRegisterDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

}

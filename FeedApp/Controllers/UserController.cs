using Microsoft.AspNetCore.Mvc;
using ASP.FeedApp.API.Models;
using ASP.FeedApp.API.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASP.FeedApp.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration, UserService userService)
        {
            _userService = userService;
            _configuration = configuration;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            var user = await _userService.GetUserByUsernameAsync(userDto.Username);

            if (user == null)
            {
                return Unauthorized(new { message = "Identifiants incorrects" });
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password);

            if (!isPasswordValid)
            {
                return Unauthorized(new { message = "Identifiants incorrects" });
            }

            Console.WriteLine("Login successful for: " + userDto.Username);
            
                        var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, userDto.Username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString, message = "Connection successful" });
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

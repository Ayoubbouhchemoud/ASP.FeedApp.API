using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ASP.FeedApp.API.Models;
using ASP.FeedApp.API.Services;
using BCrypt.Net;

namespace ASP.FeedApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public UserController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] Users user)
        {
            var collection = _mongoDbService.GetUsersCollection();

            var existingUser = await collection.Find(u => u.UserName == user.UserName).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest(new { message = "User already exists" });
            }

            user.Id = await _mongoDbService.GetNextSequenceValue("Users");

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await collection.InsertOneAsync(user);

            return Ok(new { message = "User created successfully", userId = user.Id });
        }
    }
}

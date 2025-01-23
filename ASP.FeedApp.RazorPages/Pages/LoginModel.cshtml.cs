using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASP.FeedApp.RazorPages.Models;
using ASP.FeedApp.RazorPages.Services;
using BCrypt.Net;
using MongoDB.Driver;

namespace ASP.FeedApp.RazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly MongoDbService _mongoDbService;

        public LoginModel(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        public IActionResult OnPost()
        {
            var collection = _mongoDbService.GetUsersCollection();

            var user = collection.Find(u => u.UserName == UserName).FirstOrDefault();

            if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                Message = "Invalid username or password";
                return Page();
            }

            return RedirectToPage("Welcome");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASP.FeedApp.RazorPages.Models;
using ASP.FeedApp.RazorPages.Services;
using BCrypt.Net;
using MongoDB.Driver;

namespace ASP.FeedApp.RazorPages.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly MongoDbService _mongoDbService;

        public SignUpModel(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [BindProperty]
        public Users User { get; set; }

        public IActionResult OnPost()
        {
            var collection = _mongoDbService.GetUsersCollection();

            var existingUser = collection.Find(u => u.UserName == User.UserName).FirstOrDefault();
            if (existingUser != null)
            {
                ModelState.AddModelError("", "User already exists");
                return Page();
            }

            User.Password = BCrypt.Net.BCrypt.HashPassword(User.Password);

            collection.InsertOne(User);

            return RedirectToPage("Login");
        }
    }
}

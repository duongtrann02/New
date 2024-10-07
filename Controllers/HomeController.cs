using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Project.Models;
using New_Project.Services;
using New_Project.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace New_Project.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IFirebaseService _firebaseService;

        public HomeController(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _firebaseService.GetDataAsync<Dictionary<string, Post>>("Posts");

            var sortedPosts = posts != null 
                ? new SortedDictionary<string, Post>(posts) 
                : new SortedDictionary<string, Post>();

            return View(sortedPosts.Values);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Id = Guid.NewGuid().ToString(),
                    Author = HttpContext.Session.GetString("Username"),
                    Content = model.Content,
                    PostedDate = DateTime.Now
                };

                await _firebaseService.AddPostAsync(post);
                return RedirectToAction("Index");
            }

            return View("Index");
        }
    }
}

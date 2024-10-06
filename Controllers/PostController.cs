using Microsoft.AspNetCore.Mvc;
using New_Project.Models;
using New_Project.Models.ViewModels;
using New_Project.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace New_Project.Controllers
{
    public class PostController : Controller
    {
        private readonly IFirebaseService _firebaseService;

        public PostController(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostViewModel model)
        {
            var post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                Title = model.Title,
                Content = model.Content,
                Author = HttpContext.Session.GetString("UserId")
            };
            await _firebaseService.AddPostAsync(post);

            return RedirectToAction("Index", "Home");
        }
    }
}

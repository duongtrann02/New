using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Project.Models;
using New_Project.Models.ViewModels;
using New_Project.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace New_Project.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IFirebaseService _firebaseService;

        public ProfileController(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name;

            var userEntry = await _firebaseService.GetUserByUsernameAsync(username);

            if (!userEntry.HasValue)
            {
                TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login", "Account");
            }

            var user = userEntry.Value.Value;

            var postsDict = await _firebaseService.GetDataAsync<Dictionary<string, Post>>("Posts");
            List<Post> userPosts = new List<Post>();

            if (postsDict != null)
            {
                userPosts = postsDict.Values
                                     .Where(p => p.Author == user.Username)
                                     .OrderByDescending(p => p.PostedDate)
                                     .ToList();
            }

            var viewModel = new ProfileViewModel
            {
                User = user,
                Posts = userPosts
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var username = User.Identity.Name;
            var userEntry = await _firebaseService.GetUserByUsernameAsync(username);

            if (!userEntry.HasValue)
            {
                TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login", "Account");
            }

            var user = userEntry.Value.Value;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User updatedUser, IFormFile ProfilePicture)
        {
            var username = User.Identity.Name;
            var userEntry = await _firebaseService.GetUserByUsernameAsync(username);

            if (!userEntry.HasValue)
            {
                TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login", "Account");
            }

            var user = userEntry.Value.Value;

            user.FullName = updatedUser.FullName;
            user.Bio = updatedUser.Bio;

            if (ProfilePicture != null && ProfilePicture.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);

                var imageUrl = await UploadProfilePictureAsync(ProfilePicture, fileName);

                // user.ProfilePictureUrl = imageUrl;
            }

            await _firebaseService.SetDataAsync($"users/{user.Id}", user);

            TempData["Success"] = "Cập nhật thông tin cá nhân thành công.";
            return RedirectToAction("Index");
        }

        private async Task<string> UploadProfilePictureAsync(IFormFile file, string fileName)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"/uploads/{fileName}";
            return fileUrl;
        }
    }
}

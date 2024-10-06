using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Project.Helpers;
using New_Project.Models;
using New_Project.Models.ViewModels;
using New_Project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace New_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly IFirebaseService _firebaseService;

        public AccountController(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var users = await _firebaseService.GetDataAsync<Dictionary<string, User>>("users");
                if (users == null)
                {
                    users = new Dictionary<string, User>();
                }

                if (users.Any(u => u.Value.Username == model.Username || u.Value.Email == model.Email))
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc Email đã tồn tại.");
                    return View(model);
                }

                string hashedPassword = PasswordHelper.HashPassword(model.Password);

                string userId = Guid.NewGuid().ToString();

                var newUser = new User
                {
                    Id = userId,
                    Username = model.Username,
                    Email = model.Email,
                    Password = hashedPassword,
                    FullName = model.FullName,
                    Bio = model.Bio,
                    ProfilePictureUrl = model.ProfilePictureUrl
                };

                await _firebaseService.SetDataAsync($"users/{userId}", newUser);

                await SignInUser(newUser.Username);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userEntry = await _firebaseService.GetUserByUsernameAsync(model.Username);

                if (userEntry.HasValue)
                {
                    var user = userEntry.Value.Value;

                    if (PasswordHelper.VerifyPassword(user.Password, model.Password))
                    {
                        await SignInUser(user.Username);

                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác.");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private async Task SignInUser(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties{};

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string newPassword)
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(newPassword))
            {
                ModelState.AddModelError("", "Mật khẩu không được để trống");
                return View();
            }

            bool result = await _firebaseService.UpdatePasswordAsync(userId, newPassword);
            if (result)
            {
                ViewBag.Message = "Mật khẩu đã được đổi thành công!";
            }
            else
            {
                ViewBag.Message = "Đã có lỗi xảy ra khi đổi mật khẩu.";
            }

            return View();
        }
        
    }
}

using Microsoft.AspNetCore.Mvc;
using New.Models;
using System.Text.Encodings.Web;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.AspNetCore.Identity;
using System.Net;
using New.Services;
using New.Helpers;
using FirebaseAdmin.Auth;
using System.Net.Http.Headers;
using System.Web;
using System.Formats.Asn1;
using System.Data;

namespace New.Controllers;

public class ProfileController : Controller
{
    private FirebaseService firebaseService = new FirebaseService();

    public async Task<ActionResult> Index()
    {
        if (HttpContext.Session.GetString("_username") == null || HttpContext.Session.GetString("_userId") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        var userId = HttpContext.Session.GetString("_userId");
        var user = await firebaseService.GetData<Account>($"Account/{userId}");
        if (user == null)
        {
            TempData["Error"] = "Khong tim thay nguoi dung";
            return RedirectToAction("Login", "Account");
        }
        var posts = await firebaseService.GetData<Dictionary<string, Post>>("Post");
        List<Post> userPosts = new List<Post>();
        if (posts != null)
        {
            userPosts = posts.Values.Where(p => p.Author == user.UserName)
            .OrderByDescending(p => p.PostedDate).ToList();
        }
        var viewModel = new ProfileViewModel
        {
            Account = user,
            Posts = userPosts
        };
        return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult> Edit()
    {
        if (HttpContext.Session.GetString("_username") == null || HttpContext.Session.GetString("_userId") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        var userId = HttpContext.Session.GetString("_userId");
        var user = await firebaseService.GetData<Account>($"Account/{userId}");

        if (user == null)
        {
            TempData["Error"] = "Không tìm thấy thông tin người dùng.";
            return RedirectToAction("Login", "Account");
        }
        
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(Account updatedUser)
    {
        if (HttpContext.Session.GetString("_username") == null || HttpContext.Session.GetString("_userId") == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = HttpContext.Session.GetString("_userId");
        var user = await firebaseService.GetData<Account>($"Account/{userId}");

        if (user == null)
        {
            TempData["Error"] = "Không tìm thấy thông tin người dùng.";
            return RedirectToAction("Login", "Account");
        }

        if (ModelState.IsValid)
        {
            user.AccountName = updatedUser.AccountName;
            user.DateOfBirth = updatedUser.DateOfBirth;
            user.Gender = updatedUser.Gender;
            user.Bio = updatedUser.Bio;
            user.Avatar = updatedUser.Avatar;

            await firebaseService.SetData($"Account/{userId}", user);

            HttpContext.Session.SetString("_username", HttpContext.Session.GetString("_username"));

            TempData["Success"] = "Cập nhật thông tin cá nhân thành công.";
            HttpContext.Session.SetString("_username", HttpContext.Session.GetString("_username"));
            HttpContext.Session.SetString("_userId", HttpContext.Session.GetString("_userId"));
            return RedirectToAction("Index");
        }

        return View(updatedUser);
    }

}
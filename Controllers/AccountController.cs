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


namespace New.Controllers;

public class AccountController : Controller
{
    private FirebaseService firebaseService = new FirebaseService();

    private readonly ILogger<Controller> _logger;

    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }

    public void SetSession(string username, string id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("_username")) || string.IsNullOrEmpty(HttpContext.Session.GetString("_userId")))
        {
            HttpContext.Session.SetString("_username", username);
            HttpContext.Session.SetString("_userId", id);
        }
        var _username = HttpContext.Session.GetString("_username");
        var _userId = HttpContext.Session.GetString("_userId");
        _logger.LogInformation("Session User: {User} " + _username);
        _logger.LogInformation("Session UserId: {User} " + _userId);
    }

    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(string username, string password)
    {
        var userEntry = await firebaseService.GetUserByUsername(username);
        if (userEntry.HasValue)
        {
            var user = userEntry.Value.Value;
            if (password == user.PassWord)
            {
                SetSession(user.UserName, user.Id);
                return RedirectToAction("Index", "Newsfeed");
            }
        }
        ViewBag.Message = "Ten dang nhap hoac mat khau khong dung";
        return View();
    }

    [HttpGet]
    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(Account newAccount)
    {
        var users = await firebaseService.GetData<Dictionary<string, Account>>("Account");
        if (users == null)
        {
            users = new Dictionary<string, Account>();
        }
        if (users.Any(u => u.Value.UserName == newAccount.UserName))
        {
            Console.WriteLine("username da ton tai!");
            ViewBag.Message = "Username da ton tai!";
            return View();
        }
        string userId = Guid.NewGuid().ToString();
        Console.WriteLine("user: " + userId);
        newAccount.Id = userId;
        newAccount.Bio = "Nothing here!";
        await firebaseService.SetData($"Account/{userId}", newAccount);
        HttpContext.Session.SetString("_username", newAccount.UserName);
        HttpContext.Session.SetString("_userId", userId);
        return RedirectToAction("Index", "Newsfeed");
    }

    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
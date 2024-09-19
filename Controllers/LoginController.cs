using Microsoft.AspNetCore.Mvc;
using New.Models;
using System.Text.Encodings.Web;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace New.Controllers;

public class LoginController : Controller
{

    IFirebaseConfig config = new FirebaseConfig
    {
        AuthSecret = "KARqhj67OD1LyTPzMorJuggVRW86I6SdM5tCU8bi",
        BasePath = "https://social-media-e8da3-default-rtdb.firebaseio.com/"
    };
    IFirebaseClient client;


    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Index(Account account)
    {
        try
        {
            string username = account.UserName;
            string password = account.PassWord;
            client = new FireSharp.FirebaseClient(config);
            if (client == null)
            {
                Console.WriteLine("Khong ket noi duoc voi firebase!");
                return View();
            }
            else
            {
                Console.WriteLine("Ket noi thanh cong toi firebase!");
            }
            FirebaseResponse response = await client.GetAsync("Account");
            var users = response.ResultAs<Dictionary<string, dynamic>>();
            if (users == null)
            {
                Console.WriteLine("Khong tim thay nguoi dung!");
                return View();
            }
            var user = users.FirstOrDefault(u => u.Value.UserName == username);
            if (user.Value != null)
            {
                if (user.Value.PassWord == password)
                {
                    Console.WriteLine("Dang nhap thanh cong!");
                    return RedirectToAction("Index", "Newsfeed");
                }
                else
                {
                    Console.WriteLine("Mat khau khong dung!");
                }
            }
            else
            {
                Console.WriteLine("Email khong ton tai!");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng nhập!");
            Console.WriteLine(ex.Message);
        }
        return View();
    }

}
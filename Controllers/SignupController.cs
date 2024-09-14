using Microsoft.AspNetCore.Mvc;
using New.Models;
using System.Text.Encodings.Web;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.CodeAnalysis.Elfie.Model.Map;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Web;

namespace New.Controllers;

public class SignupController : Controller
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
    public ActionResult Index(Account account)
    {
        try
        {
            CreateNewAccount(account);
            if (ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "Đăng ký thành công!";
                return RedirectToAction("Index", "Home");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng ký!");
            Console.WriteLine(ex.Message);
        }
        var data = new Account
        {
            UserName = "",
            PassWord = "",
            AccountName = "",
            Gender = ""

        };
        return View();
    }

    private void CreateNewAccount(Account account)
    {
        client = new FireSharp.FirebaseClient(config);
        if (client == null)
        {
            Console.WriteLine("Cannot connect to Firebase! createnewaccount");
            return;
        }
        else
        {
            Console.WriteLine("Connect successfully! createnewaccount");
        }
        var data = account;
        bool m = true;
        findData(data.UserName, m);
        Console.WriteLine(m + "");
        if (m == false)
        {
            Console.WriteLine("email nay da co nguoi su dung");
            TempData["SuccessMessage"] = "email nay da co nguoi su dung!";
        }
        else
        {
            PushResponse response = client.Push("Account/", data);
            data.Id = response.Result.name;
            SetResponse setResponse = client.Set("Account/" + data.Id, data);
            Console.WriteLine("email " + data.UserName + " created!");
        }


    }

    private async void findData(string n, bool m)
    {

        client = new FireSharp.FirebaseClient(config);
        if (client == null)
        {
            Console.WriteLine("Cannot connect to Firebase! countid");

        }
        else
        {
            Console.WriteLine("Connect successfully! countid");
        }
        FirebaseResponse response = await client.GetAsync("Account");
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {

            var data = response.ResultAs<Dictionary<string, dynamic>>();

            if (data != null)
            {
                foreach (var item in data)
                {
                    var username = $"{item.Value.UserName}";
                    Console.WriteLine(username + " f");
                    if (username == n)
                    {
                        m = false;
                    }
                }

            }
            else
            {
                Console.WriteLine("khong tim thay du lieu");
            }
        }
        else
        {
            Console.WriteLine("co loi xay ra khi tim du lieu tren firebase");
        }


    }
}
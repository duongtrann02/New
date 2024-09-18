using Microsoft.AspNetCore.Mvc;
using New.Models;
using System.Text.Encodings.Web;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.CodeAnalysis.Elfie.Model.Map;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Web;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Cryptography;
using System.Text;

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
        return View();
    }

    private async void CreateNewAccount(Account account)
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
        // data.PassWord = HashPassword(data.PassWord);
        bool emailExists = await CheckIfEmailExists(data.UserName);

        if (!emailExists && data.UserName.EndsWith("@gmail.com") && data.PassWord.Length>=8 && data.PassWord.Length<=32)
        {
            PushResponse response = client.Push("Account/", data);
            data.Id = response.Result.name;
            SetResponse setResponse = client.Set("Account/" + data.Id, data);
            Console.WriteLine("email " + data.UserName + " created!");
        }
        else
        {
            if(emailExists){
                Console.WriteLine("email da ton tai");
                TempData["SuccessMessage"] = "email da ton tai";
            }
            if(!data.UserName.EndsWith("@gmail.com")){
                Console.WriteLine("email khong dung dinh dang");
                TempData["SuccessMessage"] = "email khong dung dinh dang";
            }
            if(data.PassWord.Length<8 || data.PassWord.Length>32){
                Console.WriteLine("mat khau chi duoc tu 8 den 32 ki tu");
                TempData["SuccessMessage"] = "mat khau chi duoc tu 8 den 32 ki tu";
            }
            
        }


    }

    private async Task<bool> CheckIfEmailExists(string email)
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
                foreach (var item in data.Values)
                {
                    if(item.UserName == email){
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public string HashPassword(string password){
        using (var sha256 = SHA256.Create()){
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
            StringBuilder hashedPassword = new StringBuilder();
            foreach (byte b in hashedBytes)
            {
                hashedPassword.Append(b.ToString("x2"));
            }
            return hashedPassword.ToString();
        }
    }
}
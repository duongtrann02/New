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
    public async Task<ActionResult> Index(Account account)
    {
        try
        {
            client = new FireSharp.FirebaseClient(config);
            if (client == null)
            {
                Console.WriteLine("Cannot connect to Firebase! createnewaccount");
                return View();
            }
            else
            {
                Console.WriteLine("Connect successfully! createnewaccount");
            }
            var data = account;
            bool checkAccount = await CheckAccountLegit(data);

            if (checkAccount)
            {
                PushResponse response = client.Push("Account/", data);
                data.Id = response.Result.name;
                SetResponse setResponse = client.Set("Account/" + data.Id, data);
                Console.WriteLine("email " + data.UserName + " created!");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Console.WriteLine("Dang ky that bai!");
                TempData["SuccessMessage"] = "Dang ky that bai!";
                return View();
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng ký!");
            Console.WriteLine(ex.Message);
        }
        return View();
    }

    private async Task<bool> CheckAccountLegit(Account account){
        bool emailExists = await CheckIfEmailExists(account.UserName);
        if(account.UserName.EndsWith("@gmail.com")){
            if(!emailExists){
                if(account.PassWord.Length>=8 && account.PassWord.Length<=32){
                    Console.WriteLine("Account leggit!");
                    return true;
                } else Console.WriteLine("Password loi!");
            } else Console.WriteLine("Email da ton tai!");
        } else Console.WriteLine("Email loi dinh dang!");
        return false;
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
                    if (item.UserName == email)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
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
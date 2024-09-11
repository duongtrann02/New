using Microsoft.AspNetCore.Mvc;
using New.Models;
using System.Text.Encodings.Web;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;


namespace New.Controllers;

public class SignupController : Controller {
    IFirebaseConfig config = new FirebaseConfig{
        AuthSecret = "KARqhj67OD1LyTPzMorJuggVRW86I6SdM5tCU8bi", 
        BasePath = "https://social-media-e8da3-default-rtdb.firebaseio.com/"
    };
    IFirebaseClient client;
    private void ConnectToFirebase(){
        client = new FireSharp.FirebaseClient(config);
        if(client == null){
            Console.WriteLine("Cannot connect to Firebase!");
            return;
        }else{
            Console.WriteLine("Connect successfully!");
        }
    }

    public ActionResult Index(){
        return View();
    }
    [HttpPost]
    public ActionResult Index(Account account){
        try{
            CreateNewAccount(account);
            if(ModelState.IsValid){
                TempData["SuccessMessage"] = "Đăng ký thành công!";
                return RedirectToAction("Index", "Home");
            }

        }catch(Exception ex){
            ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng ký!");
            Console.WriteLine(ex.Message);
        }
        return View();
    }

    private void CreateNewAccount(Account account){
        client = new FireSharp.FirebaseClient(config);
        if(client == null){
            Console.WriteLine("Cannot connect to Firebase!");
            return;
        }
        else{
            Console.WriteLine("Connect successfully!");
        }
        var data = account;
        PushResponse response = client.Push("Account/", data);
        data.Id = int.Parse(response.Result.name);
        SetResponse setResponse = client.Set("Account/"+data.Id,data);
    }
}
using Microsoft.AspNetCore.Mvc;
using New.Models;
using System.Text.Encodings.Web;

namespace New.Controllers;

public class LoginController : Controller {
    public ActionResult Index(){
        return View();
    }

    [HttpPost]
    public ActionResult Index(Account account){
        string username = account.UserName;
        string password = account.PassWord;


        try{
            bool isAuthenticated = AuthenticateUser(username, password);
            if(isAuthenticated){
                return RedirectToAction("Index", "Home");
            }
            else{
                ModelState.AddModelError("", "2 Tên đăng nhập hoặc mật khẩu không đúng!");
                Console.WriteLine("2 Tên đăng nhập hoặc mật khẩu không đúng!");
            }

        }catch(Exception ex){
            ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng nhập!");
            Console.WriteLine(ex.Message);
        }
        return View();
    }

    private bool AuthenticateUser(string username, string password){

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)){
            throw new ArgumentException("1 Tên và mật khẩu không được để trống!");
        }

        return username == "duongtran14141@gmail.com" && password == "12345678";
    }

}
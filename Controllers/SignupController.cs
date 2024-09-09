using Microsoft.AspNetCore.Mvc;
using New.Models;
using System.Text.Encodings.Web;

namespace New.Controllers;

public class SignupController : Controller {
    // public ActionResult Index(){
    //     return View();
    // }

    public ActionResult Index(Account account){
        try{
            if(ModelState.IsValid){
                TempData["SuccessMessage"] = "Đăng ký thành công!";
                return RedirectToAction("Index", "Login");
            }

        }catch(Exception ex){
            ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng ký!");
            Console.WriteLine(ex.Message);
        }
        return View();
    }
}
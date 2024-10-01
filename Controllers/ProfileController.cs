// using Microsoft.AspNetCore.Mvc;
// using New.Models;
// using System.Text.Encodings.Web;
// using FireSharp.Config;
// using FireSharp.Interfaces;
// using FireSharp.Response;
// using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
// using Microsoft.AspNetCore.Identity;
// using System.Net;
// using New.Services;
// using New.Helpers;
// using FirebaseAdmin.Auth;
// using System.Net.Http.Headers;
// using System.Web;

// namespace New.Controllers;

// public class ProfileController : Controller
// {
//     private FirebaseService firebaseService = new FirebaseService();

//     public async Task<ActionResult> Index(){
//         if(HttpContext.Session.GetString("_username") == null){
//             return RedirectToAction("Login", "Account");
//         }


//     }

// }
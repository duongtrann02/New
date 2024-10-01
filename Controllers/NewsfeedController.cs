using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using New.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using New.Controllers;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using New.Services;

namespace New.Controllers;

public class NewsfeedController : Controller
{
    private FirebaseService firebaseService = new FirebaseService();

    

    private static List<Post> posts = new List<Post> { };

    public async Task<ActionResult> Index()
    {
        Console.WriteLine(HttpContext.Session.GetString("_username"));

        if(HttpContext.Session.GetString("_username") == null || HttpContext.Session.GetString("_userId") == null){
            return RedirectToAction("Login", "Account");
        }

        var getPosts = await firebaseService.GetAllPost();
        if (getPosts == null)
        {
            Console.WriteLine("Khong tim thay bai viet nao!");
        }
        else
        {
            posts.Clear();
            foreach (var post in getPosts)
            {
                Post newPost = new Post
                {
                    Author = post.Value.Author,
                    Content = post.Value.Content,
                    PostedDate = post.Value.PostedDate,
                    Likes = post.Value.Likes,
                    Shares = post.Value.Shares
                };

                posts.Add(newPost);
            }
        }
        return View(posts);
    }

    public ActionResult CreatePost()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> CreatePost(Post newPost)
    {
        var _post = new Post
        {
            Author = HttpContext.Session.GetString("_username"),
            Content = newPost.Content,
            PostedDate = DateTime.Now,
            Likes = 0,
            Shares = 0
        };
        await firebaseService.CreatePost($"Post/{newPost.Id}", _post);
        HttpContext.Session.SetString("_username", HttpContext.Session.GetString("_username"));
        HttpContext.Session.SetString("_userId", HttpContext.Session.GetString("_userId"));
        return RedirectToAction("Index");
    }

    public async Task<ActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return View();
        }
        var post = await firebaseService.GetPost(id);
        if (post == null)
        {
            return View(post);
        }
        return View(post);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(string id)
    {
        await firebaseService.DeletePost(id);
        return RedirectToAction("Index");
    }

}
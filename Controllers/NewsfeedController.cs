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

public class NewsfeedController : Controller
{
    IFirebaseConfig config = new FirebaseConfig
    {
        AuthSecret = "KARqhj67OD1LyTPzMorJuggVRW86I6SdM5tCU8bi",
        BasePath = "https://social-media-e8da3-default-rtdb.firebaseio.com/"
    };
    IFirebaseClient client;
    private static List<Post> posts = new List<Post> { };

    [HttpGet]
    public async Task<ActionResult> Index()
    {
        client = new FireSharp.FirebaseClient(config);
        if (client == null)
        {
            Console.WriteLine("Khong ket noi duoc toi firebase getpost");
        }
        else
        {
            Console.WriteLine("Ket noi thanh cong toi firebase getpost");
        }
        FirebaseResponse response = await client.GetAsync("Post");
        var getPosts = response.ResultAs<Dictionary<string, dynamic>>();
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

    [HttpPost]
    public async Task<ActionResult> CreatePost(string Author, string Content)
    {
        client = new FireSharp.FirebaseClient(config);
        if (client == null)
        {
            Console.WriteLine("Khong ket noi duoc toi firebase");
            return View(posts);
        }
        else
        {
            Console.WriteLine("Ket noi thanh cong toi firebase");
        }
        FirebaseResponse response = await client.GetAsync("Post");
        var newPost = new Post
        {
            Author = Author,
            Content = Content,
            PostedDate = DateTime.Now,
            Likes = 0,
            Shares = 0
        };
        PushResponse response1 = client.Push("Post/", newPost);
        newPost.Id = response1.Result.name;
        SetResponse setResponse = client.Set("Post/" + newPost.Id, newPost);
        Console.WriteLine("ID Post: " + newPost.Id);
        posts.Add(newPost);
        return RedirectToAction("Index");
    }

    private async Task GetPost()
    {
        client = new FireSharp.FirebaseClient(config);
        if (client == null)
        {
            Console.WriteLine("Khong ket noi duoc toi firebase getpost");
        }
        else
        {
            Console.WriteLine("Ket noi thanh cong toi firebase getpost");
        }
        FirebaseResponse response = await client.GetAsync("Post");
        var getPosts = response.ResultAs<Dictionary<string, dynamic>>();
        if (getPosts == null)
        {
            Console.WriteLine("Khong tim thay bai viet nao!");
            return;
        }
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

}
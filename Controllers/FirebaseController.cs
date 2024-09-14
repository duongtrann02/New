using System;
using Microsoft.AspNetCore.Mvc;
using FireSharp.Interfaces;
using FireSharp.Config;
using New.Models;
using FireSharp.Response;

namespace New.Controllers;

public class FirebaseController : Controller {
    IFirebaseConfig config = new FirebaseConfig{
        AuthSecret = "KARqhj67OD1LyTPzMorJuggVRW86I6SdM5tCU8bi", BasePath = "https://social-media-e8da3-default-rtdb.firebaseio.com/"
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
    public void Create(Account account){
        try
        {
            AddNewAccount(account);
            ModelState.AddModelError("","Add Successfully!");
        }
        catch (Exception ex)
        {
            
            ModelState.AddModelError("",ex.Message);
        }
    }
    private void AddNewAccount(Account account){
        ConnectToFirebase();
        var data = account;
        PushResponse response = client.Push("Account/", data);
        // data.Id = response.Result.name;
        SetResponse setResponse = client.Set("Account/"+data.Id,data);
    }
}
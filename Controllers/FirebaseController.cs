using System;
using Microsoft.AspNetCore.Mvc;
using FireSharp.Interfaces;
using FireSharp.Config;
using New.Models;
using FireSharp.Response;

namespace New.Controllers;

public class FirebaseController {
    private IFirebaseConfig config = new FirebaseConfig{
        AuthSecret = "KARqhj67OD1LyTPzMorJuggVRW86I6SdM5tCU8bi", BasePath = "https://social-media-e8da3-default-rtdb.firebaseio.com/"
    };
    private IFirebaseClient client;
    private FirebaseController(){
        client = new FireSharp.FirebaseClient(config);
        if(client == null){
            Console.WriteLine("Cannot connect to Firebase!");
        }else{
            Console.WriteLine("Connect successfully!");
        }
    }
    public async Task<T> GetData<T>(string path)
    {
        FirebaseResponse response = await client.GetAsync(path);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return response.ResultAs<T>();
        }
        return default(T);
    }
    public async Task SetData<T>(string path, T data)
    {
        SetResponse response = await client.SetAsync(path, data);
    }
    public async Task UpdateData<T>(string path, T data)
    {
        FirebaseResponse response = await client.UpdateAsync(path, data);
    }
    public async Task DeleteData(string path)
    {
        FirebaseResponse response = await client.DeleteAsync(path);
    }
}
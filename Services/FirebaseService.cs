using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using New.Models;
using Newtonsoft.Json;

namespace New.Services
{
    public class FirebaseService
    {
        private IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "KARqhj67OD1LyTPzMorJuggVRW86I6SdM5tCU8bi",
            BasePath = "https://social-media-e8da3-default-rtdb.firebaseio.com/"
        };

        private IFirebaseClient client;

        public FirebaseService()
        {
            client = new FireSharp.FirebaseClient(config);
            if (client == null)
            {
                throw new Exception("Cannot connect to Firebase");
            }
        }

        // Lấy dữ liệu từ Firebase
        public async Task<T> GetData<T>(string path)
        {
            FirebaseResponse response = await client.GetAsync(path);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.ResultAs<T>();
            }
            return default(T);
        }

        // Thêm hoặc cập nhật dữ liệu vào Firebase
        public async Task SetData<T>(string path, T data)
        {
            await client.SetAsync(path, data);
        }

        // Xóa dữ liệu từ Firebase
        public async Task DeleteData(string path)
        {
            await client.DeleteAsync(path);
        }

        // Lấy thông tin người dùng dựa trên Username
        public async Task<KeyValuePair<string, Account>?> GetUserByUsername(string username)
        {
            var users = await GetData<Dictionary<string, Account>>("Account");
            if (users != null)
            {
                foreach (var user in users)
                {
                    if (user.Value.UserName.Equals(username, StringComparison.OrdinalIgnoreCase))
                    {
                        return user;
                    }
                }
            }
            return null;
        }

        // Cập nhật thông tin người dùng
        public async Task UpdateUser(string userPath, Account updatedAccount)
        {
            await SetData(userPath, updatedAccount);
        }

        // Lấy dữ liệu 1 bài viết
        public async Task<KeyValuePair<string, Post>?> GetPost(string Id)
        {
            var posts = await GetData<Dictionary<string, Post>>("Post");
            if (posts != null)
            {
                foreach (var post in posts)
                {
                    if (post.Value.Id.Equals(Id, StringComparison.OrdinalIgnoreCase))
                    {
                        return post;
                    }
                }
            }
            return null;
        }

        // Lấy dữ liệu tất cả bài viết
        public async Task<Dictionary<string, Post>> GetAllPost()
        {
            FirebaseResponse response = await client.GetAsync("Post");
            return JsonConvert.DeserializeObject<Dictionary<string, Post>>(response.Body);
        }

        // Thêm bài viết mới
        public async Task CreatePost(string postPath, Post newPost)
        {
            await SetData(postPath, newPost);
        }

        // Cập nhật thông tin bài viết
        public async Task SetPost(string postPath, Post updatedPost)
        {
            await SetData(postPath, updatedPost);
        }

        // Xóa bài viết
        public async Task DeletePost(string id)
        {
            await DeleteData(id);
        }
    }
}

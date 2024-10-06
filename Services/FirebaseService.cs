using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using New_Project.Models;

namespace New_Project.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly IFirebaseConfig _config = new FirebaseConfig
        {
            AuthSecret = "KARqhj67OD1LyTPzMorJuggVRW86I6SdM5tCU8bi",
            BasePath = "https://social-media-e8da3-default-rtdb.firebaseio.com/"
        };

        private readonly IFirebaseClient _client;

        public FirebaseService()
        {
            _client = new FireSharp.FirebaseClient(_config);
            if (_client == null)
            {
                throw new Exception("Không thể kết nối với Firebase. Vui lòng kiểm tra cấu hình.");
            }
        }

        public async Task<T> GetDataAsync<T>(string path)
        {
            FirebaseResponse response = await _client.GetAsync(path);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.ResultAs<T>();
            }
            return default(T);
        }

        public async Task SetDataAsync<T>(string path, T data)
        {
            await _client.SetAsync(path, data);
        }

        public async Task DeleteDataAsync(string path)
        {
            await _client.DeleteAsync(path);
        }

        public async Task<KeyValuePair<string, User>?> GetUserByUsernameAsync(string username)
        {
            var users = await GetDataAsync<Dictionary<string, User>>("users");
            if (users != null)
            {
                foreach (var user in users)
                {
                    if (user.Value.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                    {
                        return user;
                    }
                }
            }
            return null;
        }

        public async Task<FirebaseResponse> UploadFileAsync(string path, byte[] fileBytes)
        {
            throw new NotImplementedException("Bạn cần triển khai phương thức tải lên Firebase Storage.");
        }

        public async Task<string> GetFileUrlAsync(string path)
        {
            throw new NotImplementedException("Bạn cần triển khai phương thức lấy URL từ Firebase Storage.");
        }

        public async Task AddPostAsync(Post post)
        {
            await _client.SetAsync($"Posts/{post.Id}", post);
        }

        public async Task<bool> UpdatePasswordAsync(string userId, string newPassword)
        {
            var response = await _client.UpdateAsync($"Users/{userId}/password", newPassword);
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}

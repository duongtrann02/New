using System.Collections.Generic;
using System.Threading.Tasks;
using New_Project.Models;
using FireSharp.Response;

namespace New_Project.Services
{
    public interface IFirebaseService
    {
        Task<T> GetDataAsync<T>(string path);
        Task SetDataAsync<T>(string path, T data);
        Task DeleteDataAsync(string path);
        Task<KeyValuePair<string, User>?> GetUserByUsernameAsync(string username);
        Task<FirebaseResponse> UploadFileAsync(string path, byte[] fileBytes);
        Task<string> GetFileUrlAsync(string path);
        Task AddPostAsync(Post post);
        Task<bool> UpdatePasswordAsync(string userId, string newPassword);
    }
}

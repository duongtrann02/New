// Helpers/PasswordHelper.cs
using System.Security.Cryptography;
using System.Text;

namespace New_Project.Helpers
{
    public static class PasswordHelper
    {
        // Mã hóa mật khẩu sử dụng SHA256 (Bạn nên sử dụng các thuật toán mạnh hơn)
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Chuyển đổi mật khẩu sang byte array và tính toán hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Chuyển đổi byte array sang string
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Xác thực mật khẩu
        public static bool VerifyPassword(string storedHash, string inputPassword)
        {
            string hashOfInput = HashPassword(inputPassword);
            return StringComparer.OrdinalIgnoreCase.Compare(storedHash, hashOfInput) == 0;
        }
    }
}

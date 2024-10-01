// Helpers/PasswordHelper.cs
using System.Security.Cryptography;
using System.Text;

namespace New.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            string hashOfInput = HashPassword(inputPassword);
            return string.Equals(hashedPassword, hashOfInput);
        }
    }
}

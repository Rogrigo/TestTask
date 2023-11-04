using System.Security.Cryptography;
using System.Text;

namespace TestTask.Utils
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            string salt = "super_safe_salt";

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}

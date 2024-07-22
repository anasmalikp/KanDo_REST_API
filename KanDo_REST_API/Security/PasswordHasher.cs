using System.Diagnostics.Eventing.Reader;

namespace KanDo_REST_API.Security
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public static bool VerifyPassword(string hashed, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashed);
        }
    }
}

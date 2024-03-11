using System.Security.Cryptography;
using System.Text;

namespace JWTAuthentication.Application.Test.Utils
{
    public static class HashCreator
    {
        public static string Hash(string input)
        {
            using (var sha1 = SHA1.Create())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}

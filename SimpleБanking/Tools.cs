using System.Security.Cryptography;
using System.Text;

namespace SimpleBanking
{
    public class Tools
    {
        public string HashString(string input)
        {
            using (SHA1 sha = SHA1.Create())
                return string.Join(string.Empty, sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }
    }
}
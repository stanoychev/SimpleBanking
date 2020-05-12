using System.Security.Cryptography;
using System.Text;

namespace SimpleBanking
{
    public class Tools
    {
        public byte[] HashString(string input)
        {
            using (SHA512 sha512Hash = SHA512.Create())
                return sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        }

        public bool CompareHashes(byte[] first, byte[] second)
        {
            var enumerator1 = first.GetEnumerator();
            var enumerator2 = second.GetEnumerator();
            
            while (enumerator1.MoveNext() && enumerator2.MoveNext())
                if ((byte)enumerator1.Current != (byte)enumerator2.Current)
                    return false;

            return true;
        }
    }
}
using System.Security.Cryptography;
using System.Text;

namespace Hatsukoi.Common
{
    public static class HasherExtension
    {
        public static string ToSHA256(this string origin)
        {
            byte[] source = Encoding.Default.GetBytes(origin);
            using (var mySHA256 = SHA256.Create())
            {
                byte[] hashValue = mySHA256.ComputeHash(source);
                string result = hashValue.Aggregate(string.Empty, (current, t) => current + t.ToString("X2"));
                return result.ToUpper();
            }
        }
        public static int SetRandomNum(int input)
        {
            int result = (((input * 5 + 31) * 3) + 32) * 7;
            return result;
        }
        public static int GetRandomNum(int input)
        {
            int result = ((input / 7 - 32) / 3 - 31) / 5;
            return result;
        }
    }
}

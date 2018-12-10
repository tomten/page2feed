using System;
using System.Security.Cryptography;
using System.Text;

namespace Page2Feed.Services
{

    public static class StringHelpers
    {

        public static string Md5Hex(this string s)
        {
            var md5 = MD5.Create();
            var contentBytes = Encoding.UTF8.GetBytes(s);
            var hash = md5.ComputeHash(contentBytes);
            var hashHex = BitConverter.ToString(hash);
            var hashHexPretty = hashHex.Replace("-", "").ToLowerInvariant();
            return hashHexPretty;
        }

    }

}
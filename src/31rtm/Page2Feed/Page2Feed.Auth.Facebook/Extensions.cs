using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Page2Feed.Auth.Facebook
{

    public static class Extensions
    {

        public static Uri Uri(this string uri)
        {
            return new Uri(uri);
        }

        public static T FromJson<T>(this string t)
        {
            return JsonConvert.DeserializeObject<T>(t);
        }

        public static string ToHexString(this byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", "").ToLowerInvariant();
        }

        public static byte[] HmacSha256(this byte[] key, byte[] data)
        {
            return new HMACSHA256(key).ComputeHash(data);
        }

        public static byte[] ToUtf8Bytes(this string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

    }

}
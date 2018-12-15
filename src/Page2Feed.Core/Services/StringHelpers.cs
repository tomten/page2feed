using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Page2Feed.Core.Services
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

        public static string TimeDelta(
            this TimeSpan timeSpan,
            bool past // TODO: infer automatically
            )
        {
            var delta = Math.Abs(timeSpan.TotalSeconds);

            if (delta < 1 * (60 * 1))
                return timeSpan.Seconds == 1 ? "one second" : timeSpan.Seconds + " seconds";

            if (delta < 2 * (60 * 1))
                return "a minute";

            if (delta < 45 * (60 * 1))
                return timeSpan.Minutes + " minutes";

            if (delta < 90 * (60 * 1))
                return "an hour";

            if (delta < 24 * (60 * (60 * 1)))
                return timeSpan.Hours + " hours";

            if (delta < 48 * (60 * (60 * 1)))
            {
                return past ? "yesterday" : "tomorrow";
            }

            if (delta < 30 * (24 * (60 * (60 * 1))))
                return timeSpan.Days + " days";

            if (delta < 12 * (30 * (24 * (60 * (60 * 1)))))
            {
                var months = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 30));
                return months <= 1 ? "one month" : months + " months";
            }
            var years = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 365));
            return years <= 1 ? "one year" : years + " years";
        }

    }

}
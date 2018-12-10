using System.Text.RegularExpressions;

namespace Page2Feed.Core.Services
{

    public static class RegexHelpers
    {

        public static string RegexReplace(
            this string input,
            string pattern,
            string replacement
        )
        {
            return Regex.Replace(
                input,
                pattern,
                replacement,
                RegexOptions.Singleline
            );
        }

    }

}
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Core
{
    public static class StringExtensions
    {
        static CompareInfo compareInfo = new CultureInfo("pt-BR").CompareInfo;

        public static bool ContainsInsensitive(this string source, string search)
            => compareInfo.IndexOf(source, search, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;

        public static string JustNumbers(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
           return Regex.Replace(source, @"[^\d]", string.Empty);
        }
    }
}

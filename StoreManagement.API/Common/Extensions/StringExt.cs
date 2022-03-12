using System.Text.RegularExpressions;

namespace StoreManagement.API.Common.Extensions
{
    public static class StringExt
    {
        public static bool LongerThan(this string str, int length)
        {
            return str.Length > length;
        }

        public static bool Match(this string str, string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(str);
        }
    }
}

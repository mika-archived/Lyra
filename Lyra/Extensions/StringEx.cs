using System.Text.RegularExpressions;

namespace Lyra.Extensions
{
    public static class StringEx
    {
        public static string Replace(this string str, Regex regex, string replacement)
        {
            return regex.Replace(str, replacement);
        }
    }
}
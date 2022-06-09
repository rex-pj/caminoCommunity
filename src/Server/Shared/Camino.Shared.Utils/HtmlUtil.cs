using System;
using System.Text.RegularExpressions;

namespace Camino.Shared.Utils
{
    public static class HtmlUtil
    {
        public static string TrimHtml(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
    }
}

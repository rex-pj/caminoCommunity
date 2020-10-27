using System;
using System.Text.RegularExpressions;

namespace Camino.Core.Utils
{
    public static class HtmlUtil
    {
        public static string TrimHtml(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
    }
}

namespace Camino.Shared.Utils
{
    public static class StringUtil
    {
        public static bool EqualsIgnoreCase(this string source, string target)
        {
            return source.Equals(target, StringComparison.OrdinalIgnoreCase);
        }
    }
}

namespace System.Linq
{
    public static class StringExtensions
    {
        public static bool All(this string str, Func<char, bool> predicate)
        {
            return str.ToCharArray().All(predicate);
        }

        public static bool Any(this string str, Func<char, bool> predicate)
        {
            return str.ToCharArray().Any(predicate);
        }
    }
}

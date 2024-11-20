namespace LogsQuerer.Query
{
    public static class QueryParseExtensions
    {
        public static string TrimQuotes(this string str)
        {
            return str.Trim(' ', '"', '\'', '`', '’');
        }
    }
}

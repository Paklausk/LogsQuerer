namespace LogsQuerer.Query
{
    public interface IQueryParser
    {
        /// <summary>
        /// Parses the query string into a list of query nodes.
        /// </summary>
        bool TryParse(string query, out QueryNode[]? queryNodes);
    }
}

using LogsQuerer.Logs;

namespace LogsQuerer.Query
{
    public interface IQueryExecutor
    {
        /// <summary>
        /// Execute a query and filter out logs
        /// </summary>
        QueryResult ExecuteQuery(string query, ILogFilesProvider logFilesProvider);
    }
}

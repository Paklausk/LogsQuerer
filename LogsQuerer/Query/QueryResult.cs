using LogsQuerer.Logs;

namespace LogsQuerer.Query
{
    public class QueryResult
    {
        public QueryResultStatus Status { get; set; }
        public Log[]? Logs { get; set; }
    }
}

using LogsQuerer.Common;

namespace LogsQuerer.Query
{
    public class QueryException(string message) : AppException(message)
    {
    }
}

using LogsQuerer.Query;

namespace LogsQuerer.Database
{
    public interface IQueryLogsRepository
    {
        void AuditQuery(string query, QueryResultStatus status, int logsCount);
    }
}

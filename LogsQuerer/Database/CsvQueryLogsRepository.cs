using LogsQuerer.Query;
using System.Text;

namespace LogsQuerer.Database
{
    public class CsvQueryLogsRepository : IQueryLogsRepository
    {
        const string LogsPath = "queryLogs.csv";

        public void AuditQuery(string query, QueryResultStatus status, int logsCount)
        {
            var fileInfo = new FileInfo(LogsPath);

            var sb = new StringBuilder();

            if (!fileInfo.Exists || fileInfo.Length == 0)
            {
                sb.AppendLine("query,status,logsCount,utcDate");
            }

            sb.AppendLine($"'{query}',{status},{logsCount},{DateTime.UtcNow.ToString("s")}");

            var newCsvLines = sb.ToString();

            File.AppendAllText(LogsPath, newCsvLines);
        }
    }
}

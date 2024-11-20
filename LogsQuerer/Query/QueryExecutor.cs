using LogsQuerer.Logs;

namespace LogsQuerer.Query
{
    public class QueryExecutor(IQueryParser queryParser) : IQueryExecutor
    {
        public QueryResult ExecuteQuery(string query, ILogFilesProvider logFilesProvider)
        {
            if (!queryParser.TryParse(query, out var queryNodes) || queryNodes == null || queryNodes.Length == 0)
            {
                return new QueryResult
                {
                    Status = QueryResultStatus.InvalidQuery
                };
            }

            var logFiles = logFilesProvider.GetLogFiles();

            if (logFiles.Any(logFile => !LogFileHasRequiredColumns(logFile, queryNodes)))
            {
                return new QueryResult
                {
                    Status = QueryResultStatus.ColumnNotFound
                };
            }

            var filteredLogs = FilterLogs(logFiles, queryNodes);

            return new QueryResult
            {
                Status = QueryResultStatus.Success,
                Logs = filteredLogs
            };
        }

        Log[] FilterLogs(ILogFile[] logFiles, QueryNode[] queryNodes)
        {
            var filteredLogs = new List<Log>();

            foreach (var logFile in logFiles)
            {
                if (!LogFileHasRequiredColumns(logFile, queryNodes))
                {
                    continue;
                }

                var filteredFileLogs = FilterFileLogs(logFile, queryNodes);

                filteredLogs.AddRange(filteredFileLogs);
            }

            return filteredLogs.ToArray();
        }

        List<Log> FilterFileLogs(ILogFile logFile, QueryNode[] queryNodes)
        {
            var filteredLogs = new List<Log>();

            var logFileColumns = logFile.Headers;

            foreach (var logRow in logFile.Rows)
            {
                if (LogMatchesQuery(logRow, queryNodes, logFileColumns))
                {
                    var log = CreateLog(logRow, logFileColumns);

                    filteredLogs.Add(log);
                }
            }

            return filteredLogs;
        }

        bool LogMatchesQuery(LogRow logRow, QueryNode[] queryNodes, string[] logFileColumns)
        {
            if (queryNodes == null || queryNodes.Length == 0)
            {
                // if there are no query nodes, assume match
                return true;
            }

            var andGroups = new List<List<QueryNode>>();
            var currentGroup = new List<QueryNode>();

            foreach (var node in queryNodes)
            {
                if (node.Operator == QueryOperator.Or && currentGroup.Count > 0)
                {
                    andGroups.Add(currentGroup);
                    currentGroup = new List<QueryNode>();
                }
                currentGroup.Add(node);
            }

            if (currentGroup.Count > 0)
                andGroups.Add(currentGroup);

            foreach (var group in andGroups)
            {
                // Check if all nodes in the group (AND logic) match.
                if (group.All(node => LogMatchesQueryNode(logRow, node, logFileColumns)))
                {
                    return true;
                }
            }

            return false;
        }

        bool LogMatchesQueryNode(LogRow log, QueryNode queryNode, string[] logFileColumns)
        {
            if (queryNode.Column == null || queryNode.Value == null)
            {
                return false;
            }

            int columnIndex = Array.IndexOf(logFileColumns, queryNode.Column);
            if (columnIndex == -1 || columnIndex >= log.Fields.Length)
            {
                return false;
            }

            bool match = log.Fields[columnIndex] == queryNode.Value;
            return queryNode.Not ? !match : match;
        }

        Log CreateLog(LogRow logRow, IEnumerable<string> logFileColumns)
        {
            var fields = new KeyValuePair<string, string>[logRow.Fields.Length];

            for (var i = 0; i < logRow.Fields.Length; i++)
            {
                fields[i] = new KeyValuePair<string, string>(logFileColumns.ElementAt(i), logRow.Fields[i]);
            }

            var log = new Log(fields);

            return log;
        }

        bool LogFileHasRequiredColumns(ILogFile logFile, QueryNode[] queryNodes)
        {
            foreach (var queryNode in queryNodes)
            {
                if (!logFile.Headers.Contains(queryNode.Column))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

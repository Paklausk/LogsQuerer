

using nietras.SeparatedValues;

namespace LogsQuerer.Logs
{
    public class CsvLogFile(string filePath) : ILogFile
    {
        Lazy<string[]> _headersCache = new Lazy<string[]>(() => LoadHeaders(filePath));

        Lazy<LogRow[]> _rowsCache = new Lazy<LogRow[]>(() => LoadRows(filePath));

        public string[] Headers => _headersCache.Value;

        public LogRow[] Rows => _rowsCache.Value;

        static LogRow[] LoadRows(string filePath)
        {
            using var reader = GetFileReader(filePath);

            var logRows = reader.ParallelEnumerate(row =>
            {
                var fields = new string[row.ColCount];

                for (int i = 0; i < row.ColCount; i++)
                {
                    fields[i] = row[i].ToString();
                }

                return new LogRow(fields);
            }).ToArray();

            return logRows;
        }

        static string[] LoadHeaders(string filePath)
        {
            using var reader = GetFileReader(filePath);

            var headers = reader.Header.ColNames;

            return headers.ToArray();
        }

        static SepReader GetFileReader(string filePath) => Sep.Reader(c =>
                new SepReaderOptions()
                {
                    Unescape = true,
                    HasHeader = true,
                    Sep = new Sep(','),
                }
            ).FromFile(filePath);
    }
}

namespace LogsQuerer.Logs
{
    public class CsvLogFilesProvider(IEnumerable<string> csvFilePaths) : ILogFilesProvider
    {
        Lazy<ILogFile[]> _logFilesCache = new Lazy<ILogFile[]>(() => LoadLogFiles(csvFilePaths));

        public ILogFile[] GetLogFiles() => _logFilesCache.Value;

        static ILogFile[] LoadLogFiles(IEnumerable<string> csvFilePaths)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return csvFilePaths.Select(LoadLogFile).Where(lf => lf != null).ToArray();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }

        static ILogFile? LoadLogFile(string csvFilePath)
        {
            if (!File.Exists(csvFilePath))
            {
                AnsiConsole.MarkupLine($"[yellow] Warning: File {csvFilePath} not found[/]");
                return null;
            }

            var logFile = new CsvLogFile(csvFilePath);

            if (!logFile.Headers.Any())
            {
                AnsiConsole.MarkupLine($"[yellow] Warning: File {csvFilePath} has no headers[/]");
                return null;
            }

            return logFile;
        }
    }
}

namespace LogsQuerer.Logs
{
    public interface ILogFilesProvider
    {
        /// <summary>
        /// Get all log files
        /// </summary>
        ILogFile[] GetLogFiles();
    }
}

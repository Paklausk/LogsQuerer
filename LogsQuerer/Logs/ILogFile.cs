namespace LogsQuerer.Logs
{
    public interface ILogFile
    {
        /// <summary>
        /// Log file headers
        /// </summary>
        string[] Headers { get; }

        /// <summary>
        /// Log file rows
        /// </summary>
        LogRow[] Rows { get; }
    }
}

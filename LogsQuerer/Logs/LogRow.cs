namespace LogsQuerer.Logs
{
    public readonly struct LogRow
    {
        public string[] Fields { get; }

        public LogRow(string[] fields)
        {
            Fields = fields;
        }
    }
}

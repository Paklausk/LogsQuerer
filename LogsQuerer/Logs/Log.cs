namespace LogsQuerer.Logs
{
    public readonly struct Log
    {
        public KeyValuePair<string, string>[] Fields { get; init; }

        public Log(KeyValuePair<string, string>[] fields)
        {
            Fields = fields;
        }

        public string? this[string key]
        {
            get
            {
                return Fields.FirstOrDefault(f => f.Key == key).Value;
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                return Fields.Select(f => f.Key);
            }
        }
    }
}

namespace LogsQuerer.Configuration
{
    public class Config
    {
        public IEnumerable<string>? LogFiles { get; set; }
        public int? AlertSeverityLevel { get; set; }
    }
}

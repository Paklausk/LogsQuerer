using LogsQuerer.Logs;

namespace LogsQuerer.Notifications
{
    public interface ISeverityLevelNotifier
    {
        /// <summary>
        /// Check the severity level and notify if it is higher than the configured level
        /// </summary>
        void CheckSeverityLevelAndNotify(Log[] logs, int alertSeverityLevel);
    }
}

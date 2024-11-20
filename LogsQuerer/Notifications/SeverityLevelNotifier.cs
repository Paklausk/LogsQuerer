using LogsQuerer.Logs;

namespace LogsQuerer.Notifications
{
    public class SeverityLevelNotifier(INotificationsSender notificationsSender) : ISeverityLevelNotifier
    {
        const string SeverityColumnName = "severity";

        public void CheckSeverityLevelAndNotify(Log[] logs, int alertSeverityLevel)
        {
            var severityLevelReached = logs
                .Where(log => int.TryParse(log[SeverityColumnName], out var severity) && severity >= alertSeverityLevel)
                .Any();

            if (severityLevelReached)
            {
                notificationsSender.SendNotification("[bold red]Alert! Found logs with high severity level[/]");
            }
        }
    }
}

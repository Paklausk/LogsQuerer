namespace LogsQuerer.Notifications
{
    public class ConsoleNotificationsSender : INotificationsSender
    {
        public void SendNotification(string message)
        {
            AnsiConsole.MarkupLine($"[bold yellow]Notification:[/] {message}");
        }
    }
}

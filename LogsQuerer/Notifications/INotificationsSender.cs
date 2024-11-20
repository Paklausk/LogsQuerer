namespace LogsQuerer.Notifications
{
    public interface INotificationsSender
    {
        /// <summary>
        /// Publish a notification
        /// </summary>
        void SendNotification(string message);
    }
}

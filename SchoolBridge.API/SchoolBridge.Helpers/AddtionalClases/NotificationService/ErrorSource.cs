namespace SchoolBridge.Helpers.AddtionalClases.NotificationService
{
    public class ErrorSource: MessageSource, INotificationSource
    {
        public string Id { get; set; }
        public object AdditionalInfo { get; set; }
    }
}
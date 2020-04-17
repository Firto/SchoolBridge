using SchoolBridge.Helpers.AddtionalClases.NotificationService;

namespace SchoolBridge.Helpers.DtoModels
{
    public class NotificationDto
    {
        public string Type { get; set; }
        public INotificationSource Source { get; set; }
    }
}
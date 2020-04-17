using System;

namespace SchoolBridge.Helpers.AddtionalClases.NotificationService
{
    public class DataBaseSourse : INotificationSource
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Base64Sourse { get; set; }
        public bool Read { get; set; }
    }
}
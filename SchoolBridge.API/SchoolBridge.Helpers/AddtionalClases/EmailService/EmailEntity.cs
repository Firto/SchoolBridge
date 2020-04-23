using System;
using System.Net.Mail;

namespace SchoolBridge.Helpers.AddtionalClases.EmailService
{
    public class EmailEntity
    {
        public MailMessage Message { get; set; }
        public SendCompleatedEventHandler CompletedEventHandler { get; set; }
        public object AddtionalInfo { get; set; }
        public DateTime StartSendingTime { get; set; }
    }
}

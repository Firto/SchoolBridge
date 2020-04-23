using SchoolBridge.Helpers.AddtionalClases.EmailService;
using System.Collections.Concurrent;
using System.Net.Mail;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IEmailService
    {
        ConcurrentQueue<EmailEntity> EmailQueue { get; }
        void Send(string toEmail, string FromEmail, string subject, string body, SendCompleatedEventHandler eventHandler, object AddtionalInfo);
        void SendByDraft(string toEmail, string FromEmail, string subject, string draft, SendCompleatedEventHandler eventHandler, object AddtionalInfo, params string[] arguments);
        void SendDefault(string toEmail, string subject, string body, SendCompleatedEventHandler eventHandler, object AddtionalInfo);
        void SendDefaultByDraft(string toEmail, string subject, string draft, SendCompleatedEventHandler eventHandler, object AddtionalInfo, params string[] arguments);
    }
}

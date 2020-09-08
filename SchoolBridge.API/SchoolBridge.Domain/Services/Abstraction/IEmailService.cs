using SchoolBridge.Domain.Services.Implementation;
using SchoolBridge.Helpers.AddtionalClases;
using SchoolBridge.Helpers.AddtionalClases.EmailService;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public enum EmailEntityPriority
    {
        High = 0,
        Medium,
        Low
    }
    public interface IEmailService: IOnInitService
    {
        PriorityQueue<EmailEntity> EmailQueue { get; }
        void Send(string toEmail, string FromEmail, string subject, string body, SendCompleatedEventHandler eventHandler, EmailEntityPriority priority, object AddtionalInfo);
        void SendByDraft(string toEmail, string FromEmail, string subject, string draft, SendCompleatedEventHandler eventHandler, EmailEntityPriority priority, object AddtionalInfo, params string[] arguments);
        void SendDefault(string toEmail, string subject, string body, SendCompleatedEventHandler eventHandler, EmailEntityPriority priority, object AddtionalInfo);
        void SendDefaultByDraft(string toEmail, string subject, string draft, SendCompleatedEventHandler eventHandler, EmailEntityPriority priority, object AddtionalInfo, params string[] arguments);
    }
}


using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.AddtionalClases;
using SchoolBridge.Helpers.AddtionalClases.EmailService;
using SchoolBridge.Helpers.Extentions;
using SchoolBridge.Domain.Managers.CClientErrorManager;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;


namespace SchoolBridge.Domain.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceConfiguration _configuration;
        private readonly PriorityQueue<EmailEntity> _queue = new PriorityQueue<EmailEntity>();
        public PriorityQueue<EmailEntity> EmailQueue { get => _queue; }

        public EmailService(EmailServiceConfiguration configuration) {
            _configuration = configuration;
        }

        public static void OnInit(ClientErrorManager manager) {
            manager.AddErrors(new ClientErrors("EmailService", new Dictionary<string, ClientError>
            {

            }));
        }
        private string CreateDraftPath(string name) {
            return _configuration.DraftsPath + "/" + name + ".draft.html";
        }
        private string GetDraft(string name) { 
            return System.IO.File.ReadAllText(CreateDraftPath(name));
        }

        private string ComposeDraftBody(string draftBody, params string[] arguments)
        {
            foreach (var item in arguments)
                draftBody = draftBody.ReplaceFirstOccurrance("$arg$", item);
            return draftBody;
        }

        private string ComposeDraft(string draft, params string[] arguments)
        {
            return ComposeDraftBody(GetDraft(draft), arguments);
        }

        public void Send(string toEmail, string FromEmail, string subject, string body, SendCompleatedEventHandler eventHandler, EmailEntityPriority priority, object AddtionalInfo)
        {
            MailMessage oMailMsg = new MailMessage();
            oMailMsg.To.Add(toEmail);
            oMailMsg.Subject = subject;
            oMailMsg.From = new MailAddress(FromEmail, FromEmail, Encoding.UTF8);

            oMailMsg.IsBodyHtml = true;
            oMailMsg.Body = body;

            _queue.Enqueue(new EmailEntity
            {
                Message = oMailMsg,
                StartSendingTime = DateTime.Now,
                CompletedEventHandler = eventHandler,
                AddtionalInfo = AddtionalInfo
            }, (int)priority);
        }

        public void SendByDraft(string toEmail, string FromEmail, string subject, string draft, SendCompleatedEventHandler eventHandler, EmailEntityPriority priority, object AddtionalInfo, params string[] arguments)
        {
            Send(toEmail, FromEmail, subject, ComposeDraft(draft, arguments), eventHandler, priority, AddtionalInfo);
        }

        public void SendDefault(string toEmail, string subject, string body, SendCompleatedEventHandler eventHandler, EmailEntityPriority priority, object AddtionalInfo)
        {
            Send(toEmail, _configuration.DefaultSendFrom, subject, body, eventHandler, priority, AddtionalInfo);
        }

        public void SendDefaultByDraft(string toEmail, string subject, string draft, SendCompleatedEventHandler eventHandler, EmailEntityPriority priority, object AddtionalInfo, params string[] arguments)
        {
            Send(toEmail, _configuration.DefaultSendFrom, subject, ComposeDraft(draft, arguments), eventHandler, priority, AddtionalInfo);
        }
    }
}

using SchoolBridge.DataAccess.Entities.Files;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.AddtionalClases.EmailService;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using System.Text;


namespace SchoolBridge.Domain.Services.Implementation
{
    
    public class EmailService : IEmailService
    {
        private readonly EmailServiceConfiguration _configuration;
        private readonly ConcurrentQueue<EmailEntity> _queue = new ConcurrentQueue<EmailEntity>();
        public ConcurrentQueue<EmailEntity> EmailQueue { get => _queue; }

        public EmailService(EmailServiceConfiguration configuration,
                            ClientErrorManager clientErrorManager) {
            _configuration = configuration;
            if (!clientErrorManager.IsIssetErrors("Email"))
                clientErrorManager.AddErrors(new ClientErrors("Email", new Dictionary<string, ClientError>
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
                draftBody = draftBody.Replace("$arg$", item);
            return draftBody;
        }

        private string ComposeDraft(string draft, params string[] arguments)
        {
            return ComposeDraftBody(GetDraft(draft), arguments);
        }

        public void Send(string toEmail, string FromEmail, string subject, string body, SendCompleatedEventHandler eventHandler, object AddtionalInfo)
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
            });
        }

        public void SendByDraft(string toEmail, string FromEmail, string subject, string draft, SendCompleatedEventHandler eventHandler, object AddtionalInfo, params string[] arguments)
        {
            Send(toEmail, FromEmail, subject, ComposeDraft(draft, arguments), eventHandler, AddtionalInfo);
        }

        public void SendDefault(string toEmail, string subject, string body, SendCompleatedEventHandler eventHandler, object AddtionalInfo)
        {
            Send(toEmail, _configuration.DefaultSendFrom, subject, body, eventHandler, AddtionalInfo);
        }

        public void SendDefaultByDraft(string toEmail, string subject, string draft, SendCompleatedEventHandler eventHandler, object AddtionalInfo, params string[] arguments)
        {
            Send(toEmail, _configuration.DefaultSendFrom, subject, ComposeDraft(draft, arguments), eventHandler, AddtionalInfo);
        }
    }
}

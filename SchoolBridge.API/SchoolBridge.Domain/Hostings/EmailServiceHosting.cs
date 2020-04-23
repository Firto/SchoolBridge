using Microsoft.Extensions.Hosting;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.AddtionalClases.EmailService;
using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Hostings
{
    public class EmailServiceHosting : IHostedService
    {
        private Timer _timer;
        private readonly EmailServiceConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly SmtpClient oSMTPClient;
        public EmailServiceHosting(IEmailService emailService,
                                  EmailServiceConfiguration configuration)
        {
            _configuration = configuration;
            _emailService = emailService;

            oSMTPClient = _configuration.SmtpClient;
            oSMTPClient.SendCompleted += OnCompleateSend;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                _configuration.SendEmailInterval);

            return Task.CompletedTask;
        }

        private void OnCompleateSend(object obj, AsyncCompletedEventArgs e) 
        {
            var entity = (EmailEntity)e.UserState;

            Console.WriteLine($"Sended {!e.Cancelled && e.Error == null}!");
            if (e.Error != null)
                Console.WriteLine(e.Error);

            entity.CompletedEventHandler?.Invoke(new EmailSendCompleatedEntity
                {
                    EmailEntity = entity,
                    EndSendingTime = DateTime.Now,
                    IsSended = !e.Cancelled && e.Error == null
                }
            );
        }

        private void DoWork(object state)
        {
            Console.WriteLine("Sending emails!");
            EmailEntity emailEntity = null;
            for (int i = 0; i < _configuration.MaxIntervalSendEmail && _emailService.EmailQueue.Count > 0; i++) 
                if (_emailService.EmailQueue.TryDequeue(out emailEntity))
                    oSMTPClient.SendAsync(emailEntity.Message, emailEntity);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
            => _timer?.Dispose();
    }
}

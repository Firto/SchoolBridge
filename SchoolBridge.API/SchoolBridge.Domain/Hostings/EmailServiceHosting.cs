using Microsoft.Extensions.Hosting;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.AddtionalClases.EmailService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
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
        private readonly SmtpClient[] _oSMTPClients;
        private readonly List<SmtpClient> _usingSMTPClients = new List<SmtpClient>();
        private  uint _countWorkThreads = 0;

        private object _lockObj = new object(); 
        public EmailServiceHosting(IEmailService emailService,
                                  EmailServiceConfiguration configuration)
        {
            _configuration = configuration;
            _emailService = emailService;

            _oSMTPClients = _configuration.SmtpClients;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                _configuration.SendEmailInterval);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            SmtpClient SMTPClient = null;
            lock (_lockObj) {
                if (_usingSMTPClients.Count < _oSMTPClients.Length && (_countWorkThreads < _configuration.MaxSendThreads || _configuration.MaxSendThreads == 0)) {
                    int i = 0;
                    for (; i < _oSMTPClients.Length; i++)
                    {
                        if (!_usingSMTPClients.Contains(_oSMTPClients[i])) {
                            _usingSMTPClients.Add(_oSMTPClients[i]);
                            SMTPClient = _oSMTPClients[i];
                            _countWorkThreads++;
                            break;
                        }
                    }
                    if (i == _oSMTPClients.Length)
                        return;
                }
                else return;
            }

            Console.WriteLine($"Sending emails from {((NetworkCredential)SMTPClient.Credentials).UserName}!");
            /* = new SmtpClient(oSMTPClient.Host, oSMTPClient.Port);
            SMTPClient.Credentials = oSMTPClient.Credentials;
            SMTPClient.EnableSsl = oSMTPClient.EnableSsl;
            SMTPClient.UseDefaultCredentials = oSMTPClient.UseDefaultCredentials;
            SMTPClient.DeliveryMethod = oSMTPClient.DeliveryMethod;
            SMTPClient.PickupDirectoryLocation = oSMTPClient.PickupDirectoryLocation;
            SMTPClient.DeliveryFormat = oSMTPClient.DeliveryFormat;
            SMTPClient.Timeout = oSMTPClient.Timeout;
            SMTPClient.TargetName = oSMTPClient.TargetName;*/
            EmailEntity emailEntity = null;
            bool isSended = true;
            for (int i = 0; i < _configuration.MaxSendEmailInOneThread && _emailService.EmailQueue.Count > 0; i++)
                if (_emailService.EmailQueue.TryDequeue(out emailEntity))
                {
                    isSended = true;
                    try
                    {
                        SMTPClient.Send(emailEntity.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        isSended = false;
                    }
                    Console.WriteLine($"Sended {isSended}!");
                    emailEntity.CompletedEventHandler?.Invoke(new EmailSendCompleatedEntity
                        {
                            EmailEntity = emailEntity,
                            EndSendingTime = DateTime.Now,
                            IsSended = isSended
                        }
                    );
                }
            lock (_lockObj)
            {
                _usingSMTPClients.Remove(SMTPClient);
                _countWorkThreads--;
            }
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

using System;
using System.Net.Mail;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class EmailServiceConfiguration
    {
        public SmtpClient[] SmtpClients { get; set; }
        public string DefaultSendFrom { get; set; }
        
        public string DraftsPath { get; set; }
        public string EmailServersConfigPath { get; set; }
        public TimeSpan SendEmailInterval { get; set; }
        public uint MaxSendEmailInOneThread { get; set; }
        public uint MaxSendThreads { get; set; } = 0;
    }
}

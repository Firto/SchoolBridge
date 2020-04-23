using System;
using System.Net.Mail;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class EmailServiceConfiguration
    {
        public SmtpClient SmtpClient { get; set; }
        public string DefaultSendFrom { get; set; }
        
        public string DraftsPath { get; set; }
        public TimeSpan SendEmailInterval { get; set; }
        public uint MaxIntervalSendEmail { get; set; }
    }
}

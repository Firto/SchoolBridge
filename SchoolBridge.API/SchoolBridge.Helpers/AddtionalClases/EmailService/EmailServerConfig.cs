using System.Collections.Generic;

namespace SchoolBridge.Helpers.AddtionalClases.EmailService
{
    public class EmailServerConfig
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public int DeliveryMethod { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public int Timeout { get; set; }
        public IEnumerable<EmailServerAccount> Accounts { get; set; }
    }
}

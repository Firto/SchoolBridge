using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.AddtionalClases.EmailService
{
    public class EmailSendCompleatedEntity
    {
        public EmailEntity EmailEntity { get; set; }
        public DateTime EndSendingTime { get; set; }
        public bool IsSended { get; set; }
    }
}

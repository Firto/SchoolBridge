using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.AddtionalClases.NotificationService
{
    public class OnSendEmailSource: INotificationSource
    {
        public string Email { get; set; }
        public bool Ok { get; set; }
    }
}

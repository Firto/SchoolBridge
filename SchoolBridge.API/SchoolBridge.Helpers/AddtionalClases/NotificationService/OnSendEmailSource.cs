using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.AddtionalClases.NotificationService
{
    public class OnSendEmailSource: INotificationSource
    {
        public bool Ok { get; set; }
        public string Email { get; set; }
    }
}

using SchoolBridge.Helpers.DtoModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.AddtionalClases.ChatEventService.Events
{
    public class ChatTypingEventSource: IChatEventSource
    {
        public string[] Typing { get; set; }
    }
}

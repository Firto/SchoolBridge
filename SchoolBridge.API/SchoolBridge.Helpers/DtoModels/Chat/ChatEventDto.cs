using SchoolBridge.Helpers.AddtionalClases.ChatEventService.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels.Chat
{
    public class ChatEventDto
    {
        public string Type { get; set; }
        public string ChatId { get; set; }
        public IChatEventSource Source { get; set; }
    }
}

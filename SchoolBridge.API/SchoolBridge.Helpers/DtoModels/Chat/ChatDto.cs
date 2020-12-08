using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels.Chat
{
    public abstract class ChatDto
    {
        public string Id { get; set; }
        public string SubscribeToken { get; set; }
        public bool Direct { get; set; } = true;
    }
}

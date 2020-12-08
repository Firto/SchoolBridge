using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels.Chat
{
    public class DirectChatDto: ChatDto
    {
        public bool Read { get; set; }
        public bool Typing { get; set; }
        public ShortUserDto User { get; set; }
        public MessageDto LastMessage { get; set; }
    }
}

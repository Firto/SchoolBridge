using SchoolBridge.Helpers.AddtionalClases.DataBaseNotoficationService;
using SchoolBridge.Helpers.DtoModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SchoolBridge.Helpers.DtoModels.Chat
{
    public class MessageDto
    {
        public string Id { get; set; }
        public ShortUserDto Sender { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Base64Source { get; set; }
    }
}

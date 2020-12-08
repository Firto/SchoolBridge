using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolBridge.DataAccess.Entities.Chat
{
    public class DirectMessage : ICloneable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Base64Source { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        public User Sender { get; set; }

        [ForeignKey("Chat")]
        public string ChatId { get; set; }
        public DirectChat Chat { get; set; }

        public object Clone()
            => new DirectMessage
            {
                Id = Id,
                Type = Type,
                Date = Date,
                Base64Source = Base64Source,
                SenderId = SenderId,
                ChatId = ChatId
            };
    }
}

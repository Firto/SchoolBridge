using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolBridge.DataAccess.Entities.Chat
{
    public class DirectChat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [ForeignKey("User1")]
        public string User1Id { get; set; }
        public User User1 { get; set; }

        [ForeignKey("User2")]
        public string User2Id { get; set; }
        public User User2 { get; set; }

        [Required]
        public int Read { get; set; } = 0;

        [Required]
        public DateTime LastModify { get; set; } = DateTime.Now;

        public IEnumerable<DirectMessage> Messages { get; set; }
    }
}

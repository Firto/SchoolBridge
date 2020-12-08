using SchoolBridge.DataAccess.Entities.Authorization;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class Notification : ICloneable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Type { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Base64Sourse { get; set; }
        [Required]
        public bool Read { get; set; } = false;

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        public object Clone()
            => new Notification
            {
                Id = Id,
                Type = Type,
                Date = Date,
                Base64Sourse = Base64Sourse,
                UserId = UserId
            };
    }
}
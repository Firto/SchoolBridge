using SchoolBridge.DataAccess.Entities.Authorization;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class Notification<AUser> : ICloneable where AUser : AuthUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Base64Sourse { get; set; }
        public bool Read { get; set; } = false;

        [ForeignKey("User")]
        public string UserId { get; set; }
        public AUser User { get; set; }

        public object Clone()
            => new Notification<AUser>
            {
                Id = Id,
                Type = Type,
                Date = Date,
                Base64Sourse = Base64Sourse,
                UserId = UserId
            };
    }
}
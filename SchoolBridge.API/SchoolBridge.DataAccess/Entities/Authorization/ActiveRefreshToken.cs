using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities.Authorization
{
    public class ActiveRefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Jti { get; set; }
        public string UUID { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime Expire { get; set; }
    }
}
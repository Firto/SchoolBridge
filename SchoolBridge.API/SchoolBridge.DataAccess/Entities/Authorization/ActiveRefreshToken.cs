using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities.Authorization
{
    public class ActiveRefreshToken<AUser> where AUser : AuthUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Jti { get; set; }
        public string UUID { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public AUser User { get; set; }
        public DateTime Expire { get; set; }
    }
}
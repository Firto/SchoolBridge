using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class UserPermission
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [Key]
        [ForeignKey("Permission")]
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
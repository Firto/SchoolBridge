using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class UserPermission
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Permission")]
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }

        public object Clone()
           => new UserPermission
           {
               UserId = UserId,
               User = User,
               PermissionId = PermissionId,
               Permission = Permission
           };
    }
}
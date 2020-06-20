using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class DefaultRolePermission
    {
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [ForeignKey("Permission")]
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }

        public object Clone()
            => new DefaultRolePermission
            {
                RoleId = RoleId,
                Role = Role,
                PermissionId = PermissionId,
                Permission = Permission
            };
    }
}
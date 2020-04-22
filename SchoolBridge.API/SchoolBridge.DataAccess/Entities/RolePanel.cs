using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class RolePanel
    {
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [ForeignKey("Panel")]
        public int PanelId { get; set; }
        public Panel Panel { get; set; }

        public object Clone()
            => new RolePanel
            {
                RoleId = RoleId,
                Role = Role,
                PanelId = PanelId,
                Panel = Panel
            };
    }
}
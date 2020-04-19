using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class RolePanel
    {
        [Key]
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [Key]
        [ForeignKey("Panel")]
        public int PanelId { get; set; }
        public Panel Panel { get; set; }
    }
}
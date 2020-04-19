using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class UserPanel
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [Key]
        [ForeignKey("Panel")]
        public int PanelId { get; set; }
        public Panel Panel { get; set; }
    }
}
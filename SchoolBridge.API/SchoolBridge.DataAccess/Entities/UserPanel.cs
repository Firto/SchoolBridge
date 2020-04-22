using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class UserPanel
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Panel")]
        public int PanelId { get; set; }
        public Panel Panel { get; set; }

        public object Clone()
            => new UserPanel
            {
                UserId = UserId,
                User = User,
                PanelId = PanelId,
                Panel = Panel
            };
    }
}

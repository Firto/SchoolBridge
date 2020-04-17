using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class SchoolTeacher
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("User")]
        public string SchoolId { get; set; }
        public School School { get; set; }
    }
}

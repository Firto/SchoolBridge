using SchoolBridge.DataAccess.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenP.DataAccess.Entities
{
    public class TeacherSubject
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
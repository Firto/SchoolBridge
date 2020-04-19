using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class SchoolUnit // SchoolHeadTeachers, SchoolModerators
    {
        [Key]
        [ForeignKey("User")]
        public string SchoolId { get; set; }
        public School School { get; set; }

        [Key]
        [ForeignKey("Unit")]
        public string UnitId { get; set; }
        public User Unit { get; set; }
    }
}
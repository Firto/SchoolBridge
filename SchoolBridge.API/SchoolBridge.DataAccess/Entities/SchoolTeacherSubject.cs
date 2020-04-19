using SchoolBridge.DataAccess.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class SchoolTeacherSubject
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Teacher")]
        public string TeacherId { get; set; }
        public SchoolTeacher Teacher { get; set; }

        [ForeignKey("Subject")]
        public string SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
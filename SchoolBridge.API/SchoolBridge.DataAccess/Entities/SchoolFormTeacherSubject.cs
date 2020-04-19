using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class SchoolFormTeacherSubject
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Form")]
        public int FormId { get; set; }
        public SchoolForm Form { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public SchoolTeacherSubject Subject { get; set; }
    }
}

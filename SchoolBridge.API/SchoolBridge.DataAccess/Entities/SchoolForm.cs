using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class SchoolForm
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public SchoolFormTemlate Template { get; set; }

        [ForeignKey("Semester")]
        public int SemesterId { get; set; }
        public SchoolSemester Semester { get; set; }
    }
}
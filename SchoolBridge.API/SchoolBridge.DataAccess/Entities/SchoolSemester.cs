using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class SchoolSemester
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public SchoolSemesterTemlate Template { get; set; }

        [ForeignKey("User")]
        public string SchoolId { get; set; }
        public School School { get; set; }

        [Required]
        public int StartYear { get; set; }
    }
}
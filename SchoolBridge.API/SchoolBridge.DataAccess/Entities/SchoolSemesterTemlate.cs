using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class SchoolSemesterTemlate
    {
        [Key]
        public string Name { get; set; }

        [Required]
        public byte StartMonth { get; set; }
        [Required]
        public byte StartDay { get; set; }
        [Required]
        public byte EndMonth { get; set; }
        [Required]
        public byte EndDay { get; set; }
    }
}

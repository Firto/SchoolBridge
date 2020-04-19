using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class SchoolFormTemlate
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("School")]
        public string SchoolId { get; set; }
        public School School { get; set; }

        [Required]
        public byte Form { get; set; }
        [Required]
        public byte Char { get; set; }
    }
}

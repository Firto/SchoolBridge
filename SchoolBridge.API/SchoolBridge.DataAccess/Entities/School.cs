using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class School
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required, MaxLength(120)]
        public string Name { get; set; }
        [Required, MaxLength(120)]
        public string Region { get; set; } // Область
        [Required, MaxLength(120)]
        public string District { get; set; } // Район

        [ForeignKey("Director")]
        public string DirectorId { get; set; }
        public User Director { get; set; }
    }
}

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
        [ForeignKey("RegionDistinct")]
        public int RegionDistinctId { get; set; }
        public RegionDistinct RegionDistinct { get; set; }

        [ForeignKey("Director")]
        public string DirectorId { get; set; }
        public User Director { get; set; }
    }
}

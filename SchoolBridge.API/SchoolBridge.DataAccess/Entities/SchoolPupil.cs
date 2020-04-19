using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class SchoolPupil
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("School")]
        public string SchoolId { get; set; }
        public School School { get; set; }

        [ForeignKey("Pupil")]
        public string PupilId { get; set; }
        public User Pupil { get; set; }
    }
}
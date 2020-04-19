using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class SchoolPupilForm
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Pupil")]
        public string PupilId { get; set; }
        public SchoolPupil Pupil { get; set; }

        [ForeignKey("Form")]
        public string FormId { get; set; }
        public SchoolForm Form { get; set; }
    }
}

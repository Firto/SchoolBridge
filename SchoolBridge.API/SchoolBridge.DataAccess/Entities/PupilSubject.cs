using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolBridge.DataAccess.Entities
{
    public class PupilSubject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SubjectName { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public byte DayNumber { get; set; }
        [Required]
        public byte LessonNumber { get; set; }

        [ForeignKey("Pupil")]
        public string PupilId { get; set; }
        public User Pupil { get; set; }
    }
}

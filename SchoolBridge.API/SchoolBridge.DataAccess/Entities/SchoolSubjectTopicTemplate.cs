using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolBridge.DataAccess.Entities
{
    public class SchoolSubjectTopicTemplate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public byte Form { get; set; }
        [Required, MaxLength(120)]
        public string Name { get; set; }
        [ForeignKey("Subject")]
        public string SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}

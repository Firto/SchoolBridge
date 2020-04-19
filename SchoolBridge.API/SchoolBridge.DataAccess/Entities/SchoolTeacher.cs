﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class SchoolTeacher
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("Teacher")]
        public string TeacherId { get; set; }
        public User Teacher { get; set; }

        [ForeignKey("User")]
        public string SchoolId { get; set; }
        public School School { get; set; }
    }
}

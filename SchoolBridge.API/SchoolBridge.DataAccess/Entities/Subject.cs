using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolBridge.DataAccess.Entities
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(120)]
        public string Name { get; set; }
    }
}
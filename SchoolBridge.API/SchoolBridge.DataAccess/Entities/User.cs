﻿using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.DataAccess.Entities.Files.Images;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class User : AuthUser
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool EmailConfirmed { get; set; } = false;
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Lastname { get; set; }
        [Required]
        public string PasswordHash { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [ForeignKey("Photo")]
        public string PhotoId { get; set; }
        public Image Photo { get; set; }

        public IEnumerable<Notification<User>> Notifications { get; set; }
    }
}
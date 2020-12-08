using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels
{
    public class ProfileDto
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Photo { get; set; }
        public string Bio { get; set; }
        public uint Experience { get; set; }
    }
}

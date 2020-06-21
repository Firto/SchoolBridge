using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels
{
    public class ChangePasswordDto
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}

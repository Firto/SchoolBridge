using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels
{
    public class EndChangePasswordEmailDto
    {
        public string changePasswordToken { get; set; }
        public string newPassword { get; set; }
    }
}

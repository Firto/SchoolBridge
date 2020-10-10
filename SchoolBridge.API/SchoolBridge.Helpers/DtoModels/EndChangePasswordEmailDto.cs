using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels
{
    public class EndChangePasswordEmailDto
    {
        [PropValid("str-input")]
        public string changePasswordToken { get; set; }
        [PropValid("str-input", "str-password")]
        public string newPassword { get; set; }
    }
}

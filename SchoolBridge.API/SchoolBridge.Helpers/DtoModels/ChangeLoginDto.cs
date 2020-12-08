using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels
{
    public class ChangeLoginDto
    {
        [PropValid("str-input", "str-login")]
        public string Login { get; set; }
    }
}

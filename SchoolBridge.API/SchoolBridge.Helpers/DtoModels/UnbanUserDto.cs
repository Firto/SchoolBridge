using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels
{
    public class UnbanUserDto
    {
        [PropValid("str-input", "str-guid")]
        public string UserId { get; set; }
    }
}

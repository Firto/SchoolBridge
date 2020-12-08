using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels
{
    public class ChangeImageDto
    {
        [PropValid("str-input")]
        public string Image { get; set; }
    }
}

using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels.Globalization
{
    public class AddOrUpdateStringDto
    {
        [PropValid("str-input", "lss-str-id-name")]
        public string Name { get; set; }
        [PropValid("str-input", "lss-lng-full-name")]
        public string String { get; set; }
    }
}

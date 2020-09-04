using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels.Globalization
{
    public class GetLanguageEditDto
    {
        [PropValid("str-input", "lss-lng-name")]
        public string AbbName { get; set; }
        [PropValid("str-input", "lss-lng-full-name")]
        public string FullName { get; set; }
    }
}

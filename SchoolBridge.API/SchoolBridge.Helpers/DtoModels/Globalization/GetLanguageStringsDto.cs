using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels.Globalization
{
    public class GetLanguageStringsDto
    {
        [PropValid("str-input", "lss-lng-type")]
        public string[] Types { get; set; }
    }
}

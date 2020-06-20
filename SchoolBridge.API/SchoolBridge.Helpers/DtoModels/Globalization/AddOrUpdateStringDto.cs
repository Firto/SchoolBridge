﻿using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels.Globalization
{
    public class AddOrUpdateStringDto
    {
        [PropValid("str-input", "lss-lng-name")]
        public string LangAbbName { get; set; }
        [PropValid("str-input", "lss-str-id-name")]
        public string Name { get; set; }
        [PropValid("str-input", "lss-lng-type")]
        public string[] Types { get; set; }
        [PropValid("str-input", "lss-str-name")]
        public string String { get; set; }
    }
}

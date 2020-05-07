using SchoolBridge.Domain.Services.Implementation;
using System;
using System.Collections.Generic;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class ValidatingServiceConfiguration
    {
        public IDictionary<Type, ValidateFunc> ValidateFunctions { get; set; } = new Dictionary<Type, ValidateFunc>();
        public short MaxCountCharsLogin { get; set; } = 25;
        public short MaxCountCharsPassword { get; set; } = 25;
        public short MaxCountCharsName { get; set; } = 60;
        public short MaxCountCharsSurname { get; set; } = 255;
        public short MaxCountCharsLastname { get; set; } = 70;
        public short MinCountCharsPassword { get; set; } = 8;
    }
}

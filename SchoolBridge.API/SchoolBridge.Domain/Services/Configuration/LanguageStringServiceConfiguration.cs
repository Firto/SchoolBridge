using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class LanguageStringServiceConfiguration
    {
        public string DefaultLanguage { get; set; }
        public int MaxLangFullNameLength { get; set; } = 40;
        public int MaxTypeNameLength { get; set; } = 40;
        public int MaxStringIdNameLength { get; set; } = 40;
        public int MaxStringLength { get; set; } = 100;

        public string BaseUpdId { get; set; } = Guid.NewGuid().ToString();
    }
}

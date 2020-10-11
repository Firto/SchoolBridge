using System;
using System.Collections.Generic;
using System.Text;
using SchoolBridge.Domain.Services.Abstraction;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class LanguageStringServiceConfiguration: IMyService
    {
        public string DefaultLanguage { get; set; }
        public int MaxLangFullNameLength { get; set; } = 300;
        public int MaxTypeNameLength { get; set; } = 40;
        public int MaxStringIdNameLength { get; set; } = 100;
        public int MaxStringLength { get; set; } = 300;

        public string BaseUpdId { get; set; } = Guid.NewGuid().ToString();
    }
}

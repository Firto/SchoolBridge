using System.Collections.Generic;

namespace SchoolBridge.Helpers.DtoModels.Globalization
{
    public class GlobalizationInfoDto
    {
        public string BaseUpdateId { get; set; }

        public string CurrentLanguage { get; set; }
        public IDictionary<string, string> AwaibleLanguages { get; set; }
    }
}

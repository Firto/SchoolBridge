using SchoolBridge.Helpers.AddtionalClases.ValidatingService;

namespace SchoolBridge.Helpers.DtoModels.Globalization
{
    public class AddLanguageDto
    {
        [PropValid("str-input", "lss-lng-name")]
        public string AbbName { get; set; }
        [PropValid("str-input", "lss-lng-full-name")]
        public string FullName { get;set; }
    }
}

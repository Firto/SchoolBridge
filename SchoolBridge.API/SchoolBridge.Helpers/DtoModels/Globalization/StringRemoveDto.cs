using SchoolBridge.Helpers.AddtionalClases.ValidatingService;

namespace SchoolBridge.Helpers.DtoModels.Globalization
{
    public class StringRemoveDto
    {
        [PropValid("str-input", "lss-str-id-name")]
        public string String { get; set; }
    }
}

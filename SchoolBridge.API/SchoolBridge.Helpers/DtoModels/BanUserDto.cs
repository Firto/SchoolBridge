using SchoolBridge.Helpers.AddtionalClases.ValidatingService;

namespace SchoolBridge.Helpers.DtoModels
{
    public class BanUserDto
    {
        [PropValid("str-input", "str-guid")]
        public string UserId { get; set; }
        [PropValid("str-input", "str-reason")]
        public string Reason { get; set; }
    }
}

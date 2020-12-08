using SchoolBridge.Helpers.AddtionalClases.ValidatingService;

namespace SchoolBridge.Helpers.DtoModels.Authefication
{
    public class StartRegisterDto
    {
        [PropValid("str-input", "str-email", "str-email-reg-no")]
        public string Email { get; set; }
    }
}
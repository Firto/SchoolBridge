using SchoolBridge.Helpers.AddtionalClases.ValidatingService;

namespace SchoolBridge.Helpers.DtoModels.Authefication
{
    public class LoginDto
    {
        [PropValid("str-input")]
        public string Login { get; set; }
        [PropValid("str-input")]
        public string Password { get; set; }
    }
}
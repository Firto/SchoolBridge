using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using System;

namespace SchoolBridge.Helpers.DtoModels.Authefication
{
    public class EndRegisterDto
    {
        [PropValid("str-input")]
        public string RegistrationToken { get; set; }

        [PropValid("str-input", "str-login")]
        public string Login { get; set; }
        [PropValid("str-input", "str-creds")]
        public string Name { get; set; }
        [PropValid("str-input", "str-creds")]
        public string Surname { get; set; }
        [PropValid("str-input", "str-creds")]
        public string Lastname { get; set; }
        [PropValid("not-null", "date-birthday")]
        public DateTime? Birthday { get; set; }
        [PropValid("str-input", "str-password")]
        public string Password { get; set; }
        [PropValid("str-input", "str-password-rep")]
        public string ConfirmPassword { get; set; }
    }
}
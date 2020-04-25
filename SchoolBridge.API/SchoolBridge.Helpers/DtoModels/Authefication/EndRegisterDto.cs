namespace SchoolBridge.Helpers.DtoModels.Authefication
{
    public class EndRegisterDto
    {
        public string RegistrationToken { get; set; }

        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
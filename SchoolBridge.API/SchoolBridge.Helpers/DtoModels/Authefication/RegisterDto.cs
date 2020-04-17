namespace SchoolBridge.Helpers.DtoModels.Authefication
{
    public class RegisterDto
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
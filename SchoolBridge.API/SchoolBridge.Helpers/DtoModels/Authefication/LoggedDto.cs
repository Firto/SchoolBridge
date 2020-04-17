namespace SchoolBridge.Helpers.DtoModels.Authefication
{
    public class LoggedDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public long Expires { get; set; }
    }
}
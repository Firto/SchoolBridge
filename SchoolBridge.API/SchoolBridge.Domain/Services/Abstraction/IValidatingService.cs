namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IValidatingService
    {
        void ValidateLogin(string login);
        void ValidateName(string name);
        void ValidateSurname(string surname);
        void ValidateLastname(string lastname);
        void ValidatePassword(string password);
        void ValidateEmail(string email);
        void ValidateRepeatPassword(string password, string repeat);
    }
}

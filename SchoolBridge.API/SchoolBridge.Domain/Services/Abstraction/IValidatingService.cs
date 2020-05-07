using SchoolBridge.Domain.Services.Implementation;
using System;
using System.Collections.Generic;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IValidatingService
    {
        void Validate<T>(T dto);
        void Validate(Type type, object dto);
        bool IsIssetValidateFunc<T>();
        void AddValidateFunc<T>(ValidateFunc func);
        void ValidateLogin(string login, string propName, ref IDictionary<string, string> valid);
        void ValidateName(string name, string propName, ref IDictionary<string, string> valid);
        void ValidateSurname(string surname, string propName, ref IDictionary<string, string> valid);
        void ValidateLastname(string lastname, string propName, ref IDictionary<string, string> valid);
        void ValidatePassword(string password, string propName, ref IDictionary<string, string> valid);
        void ValidateEmail(string email, string propName, ref IDictionary<string, string> valid);
        void ValidateRepeatPassword(string password, string repeat, string propName, ref IDictionary<string, string> valid);
    }
}

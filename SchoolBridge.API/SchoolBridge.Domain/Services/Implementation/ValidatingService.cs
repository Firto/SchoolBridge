using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SchoolBridge.Domain.Services.Implementation
{
    public delegate IDictionary<string, string> ValidateFunc(IValidatingService ser, object dto);
    public class ValidatingService: IValidatingService
    {
        private readonly IUserService _userService;
        private readonly ValidatingServiceConfiguration _configuration;
        
        public ValidatingService(IUserService userService,
                                ValidatingServiceConfiguration configuration,
                                ClientErrorManager clientErrorManager) {
            _userService = userService;
            _configuration = configuration;

            if (!clientErrorManager.IsIssetErrors("Validating"))
                clientErrorManager.AddErrors(new ClientErrors("Validating", new Dictionary<string, ClientError>{
                    {"v-func-no", new ClientError("No validation function to dto!")},
                    {"v-dto-invalid", new ClientError("Invalid dto!")}
                }));
        }

        public void Validate<T>(T dto) {
            Validate(typeof(T), dto);
        }

        public void Validate(Type type, object dto) {
            if (!_configuration.ValidateFunctions.ContainsKey(type))
                throw new ClientException("v-func-no");

            IDictionary<string, string> valid = _configuration.ValidateFunctions[type](this, dto);

            if (valid.Count > 0)
                throw new ClientException("v-dto-invalid", valid);
        }

        public bool IsIssetValidateFunc<T>()
            => _configuration.ValidateFunctions.ContainsKey(typeof(T));

        public void AddValidateFunc<T>(ValidateFunc func)
            => _configuration.ValidateFunctions.Add(typeof(T), func);

        public void ValidateLogin(string login, string propName, ref IDictionary<string, string> valid)
        {
            if (string.IsNullOrEmpty(login))
                valid.Add(propName, "Input your login!");
            else if (login.Length > _configuration.MaxCountCharsLogin)
                valid.Add(propName, $"Too long login(max {_configuration.MaxCountCharsLogin} characters)!");
            else if (!Regex.Match(login, "^[a-zA-Z_0-9]*$").Success)
                valid.Add(propName, "Name musn't have specials chars!");
            else if (_userService.IsIssetByLogin(login))
                valid.Add(propName, "User with this login is already registered!");
        }

        public void ValidateName(string name, string propName, ref IDictionary<string, string> valid)
        {
            if (string.IsNullOrEmpty(name))
                valid.Add(propName, "Input your name!");
            else if (name.Length > _configuration.MaxCountCharsName)
                valid.Add(propName, $"Too long name(max {_configuration.MaxCountCharsName} characters)!");
            else if (!Regex.Match(name, "^[а-яА-ЯҐґЄєІіЇї]+$").Success)
                valid.Add(propName, "Name musn't have specials chars!");
        }

        public void ValidateSurname(string surname, string propName, ref IDictionary<string, string> valid)
        {
            if (string.IsNullOrEmpty(surname))
                valid.Add(propName, "Input your surname!");
            else if (surname.Length > _configuration.MaxCountCharsSurname)
                valid.Add(propName, $"Too long surname(max {_configuration.MaxCountCharsSurname} characters)!");
            else if (!Regex.Match(surname, "^[а-яА-ЯҐґЄєІіЇї]+$").Success)
                valid.Add(propName, "Surname musn't have specials chars!");
        }

        public void ValidateLastname(string lastname, string propName, ref IDictionary<string, string> valid)
        {
            if (string.IsNullOrEmpty(lastname))
                valid.Add(propName, "Input your lastname!");
            else if (lastname.Length > _configuration.MaxCountCharsLastname)
                valid.Add(propName, $"Too long lastname(max {_configuration.MaxCountCharsLastname} characters)!");
            else if (!Regex.Match(lastname, "^[а-яА-ЯҐґЄєІіЇї]+$").Success)
                valid.Add(propName, "Lastname musn't have specials chars!");
        }

        public void ValidatePassword(string password, string propName, ref IDictionary<string, string> valid)
        {
            if (string.IsNullOrEmpty(password))
                valid.Add(propName, "Input your password!");
            else if (password.Length > _configuration.MaxCountCharsPassword)
                valid.Add(propName, $"Too long password(max{_configuration.MaxCountCharsPassword} characters)!");
            else if (password.Length < _configuration.MinCountCharsPassword)
                valid.Add(propName, $"Password must have {_configuration.MinCountCharsPassword} and more chars!");
            else if (!password.Any(c => char.IsDigit(c)))
                valid.Add(propName, "Password must have minimum one digit!");
        }

        public void ValidateEmail(string email, string propName, ref IDictionary<string, string> valid)
        {
            if (string.IsNullOrEmpty(email))
                valid.Add(propName, "Input your email!");
            else if (!Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                valid.Add(propName, "Incorrect email!");
            else if (_userService.IsIssetByEmail(email))
                valid.Add(propName, "User with this email is already registered!");
        }

        public void ValidateRepeatPassword(string password, string repeat, string propName, ref IDictionary<string, string> valid) {
            if (string.IsNullOrEmpty(repeat))
                valid.Add(propName, "Input repeat your password!");
            else if (repeat != password)
                valid.Add(propName, "Incorrect repeat password!");
        }

    }
}

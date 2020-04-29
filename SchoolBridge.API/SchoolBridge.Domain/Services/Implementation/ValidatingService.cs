using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class ValidatingService: IValidatingService
    {
        private readonly IUserService _userService;
        private readonly ValidatingServiceConfiguration _configuration;

        public ValidatingService(IUserService userService,
                                ValidatingServiceConfiguration configuration,
                                ClientErrorManager clientErrorManager) {
            _userService = userService;
            _configuration = configuration;

            if (!clientErrorManager.IsIssetErrors("Registration"))
                clientErrorManager.AddErrors(new ClientErrors("Validating", new Dictionary<string, ClientError>{
                    {"v-login-input", new ClientError("Input your login!")},
                    {"v-login-too-long", new ClientError($"Too long login(max {_configuration.MaxCountCharsLogin} characters)!")},
                    {"v-login-spec-chars", new ClientError("Name musn't have specials chars!")},
                    {"v-login-alrd-reg", new ClientError("User with this login is already registered!")},

                    {"v-name-input", new ClientError("Input your name!")},
                    {"v-name-too-long", new ClientError($"Too long name(max {_configuration.MaxCountCharsName} characters)!")},
                    {"v-name-spec-chars", new ClientError("Name musn't have specials chars!")},

                    {"v-surname-input", new ClientError("Input your surname!")},
                    {"v-surname-too-long", new ClientError($"Too long surname(max {_configuration.MaxCountCharsSurname} characters)!")},
                    {"v-surname-spec-chars", new ClientError("Surname musn't have specials chars!")},

                    {"v-lastname-input", new ClientError("Input your lastname!")},
                    {"v-lastname-too-long", new ClientError($"Too long lastname(max {_configuration.MaxCountCharsLastname} characters)!")},
                    {"v-lastname-spec-chars", new ClientError("Lastname musn't have specials chars!")},

                    {"v-email-input", new ClientError("Input your email!")},
                    {"v-email-inc", new ClientError("Incorrect email!")},
                    {"v-email-alrd-reg", new ClientError("User with this email is already registered!")},

                    {"v-pass-input", new ClientError("Input your password!")},
                    {"v-pass-cou-chars", new ClientError($"Password must have {_configuration.MinCountCharsPassword} and more chars!")},
                    {"v-pass-cou-digit", new ClientError("Password must have minimum one digit!")},
                    {"v-pass-too-long", new ClientError($"Too long password(max {_configuration.MaxCountCharsPassword} characters)!")},

                    {"v-pass-repeat-input", new ClientError("Input repeat your password!")},
                    {"v-pass-repeat-inc", new ClientError("Passwords do not match!")}
                }));
        }

        public void ValidateLogin(string login)
        {
            if (string.IsNullOrEmpty(login))
                throw new ClientException("v-login-input");
            else if (login.Length > _configuration.MaxCountCharsLogin)
                throw new ClientException("v-login-too-long");
            else if (!Regex.Match(login, "^[a-zA-Z_0-9]*$").Success)
                throw new ClientException("v-login-spec-chars");
            else if (_userService.IsIssetByLogin(login))
                throw new ClientException("v-login-alrd-reg");
        }

        public void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ClientException("v-name-input");
            else if (name.Length > _configuration.MaxCountCharsName)
                throw new ClientException("v-name-too-long");
            else if (!Regex.Match(name, "^[а-яА-ЯҐґЄєІіЇї]+$").Success)
                throw new ClientException("v-name-spec-chars");
        }

        public void ValidateSurname(string surname)
        {
            if (string.IsNullOrEmpty(surname))
                throw new ClientException("v-surname-input");
            else if (surname.Length > _configuration.MaxCountCharsSurname)
                throw new ClientException("v-surname-too-long");
            else if (!Regex.Match(surname, "^[а-яА-ЯҐґЄєІіЇї]+$").Success)
                throw new ClientException("v-surname-spec-chars");
        }

        public void ValidateLastname(string lastname)
        {
            if (string.IsNullOrEmpty(lastname))
                throw new ClientException("v-lastname-input");
            else if (lastname.Length > _configuration.MaxCountCharsLastname)
                throw new ClientException("v-lastname-too-long");
            else if (!Regex.Match(lastname, "^[а-яА-ЯҐґЄєІіЇї]+$").Success)
                throw new ClientException("v-lastname-spec-chars");
        }

        public void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ClientException("v-pass-input");
            else if (password.Length > _configuration.MaxCountCharsPassword)
                throw new ClientException("v-pass-too-long");
            else if (password.Length < _configuration.MinCountCharsPassword)
                throw new ClientException("v-pass-cou-chars");
            else if (!password.Any(c => char.IsDigit(c)))
                throw new ClientException("v-pass-cou-digit");
        }

        public void ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ClientException("v-email-input");
            else if (!Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                throw new ClientException("v-email-inc");
            else if (_userService.IsIssetByEmail(email))
                throw new ClientException("v-email-alrd-reg");
        }

        public void ValidateRepeatPassword(string password, string repeat) {
            if (string.IsNullOrEmpty(repeat))
                throw new ClientException("v-pass-repeat-input");
            else if (repeat != password)
                throw new ClientException("v-pass-repeat-inc");
        }

    }
}

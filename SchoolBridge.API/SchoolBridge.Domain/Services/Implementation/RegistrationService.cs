using Microsoft.IdentityModel.Tokens;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.AddtionalClases.EmailService;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.DtoModels.Authefication;
using SchoolBridge.Helpers.Managers;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRoleService _roleService;
        private readonly IEmailService _emailService;
        private readonly INotificationService<User> _notificationService;
        private readonly IUserService _userService;
        private readonly RegistrationServiceConfiguration _configuration;
        public RegistrationService(IRoleService roleService,
                                    IEmailService emailService,
                                    INotificationService<User> notificationService,
                                    IUserService userService,
                                    RegistrationServiceConfiguration configuration,
                                    ClientErrorManager clientErrorManager)
        {
            _roleService = roleService;
            _emailService = emailService;
            _userService = userService;
            _notificationService = notificationService;
            _configuration = configuration;

            if (!clientErrorManager.IsIssetErrors("Registration"))
                clientErrorManager.AddErrors(new ClientErrors("Registration", new Dictionary<string, ClientError>
                {
                    { "inc-registration-token", new ClientError("Incorrect registration token!")},
                    {"r-input-login", new ClientError("Input your login!")},
                    {"too-long-login", new ClientError($"Too long login(max {_configuration.MaxCountCharsLogin} characters)!")},
                    {"login-spec-chars", new ClientError("Name musn't have specials chars!")},

                    {"r-input-name", new ClientError("Input your name!")},
                    {"too-long-name", new ClientError($"Too long name(max {_configuration.MaxCountCharsName} characters)!")},
                    {"name-spec-chars", new ClientError("Name musn't have specials chars!")},

                    {"r-input-surname", new ClientError("Input your surname!")},
                    {"too-long-surname", new ClientError($"Too long surname(max {_configuration.MaxCountCharsSurname} characters)!")},
                    {"surname-spec-chars", new ClientError("Surname musn't have specials chars!")},

                    {"r-input-lastname", new ClientError("Input your lastname!")},
                    {"too-long-lastname", new ClientError($"Too long lastname(max {_configuration.MaxCountCharsLastname} characters)!")},
                    {"lastname-spec-chars", new ClientError("Lastname musn't have specials chars!")},

                    {"r-input-pass", new ClientError("Input your password!")},
                    {"pass-count-chars", new ClientError($"Password must have {_configuration.MinCountCharsPassword} and more chars!")},
                    {"pass-count-digit", new ClientError("Password must have minimum one digit!")},
                    {"too-long-pass", new ClientError($"Too long password(max {_configuration.MaxCountCharsPassword} characters)!")},
                    {"input-repeat-pass", new ClientError("Input repeat your password!")},
                    {"inc-repeat-pass", new ClientError("Passwords do not match!")},
                    {"already-registered", new ClientError("User with this login is already registered!")},
                    {"reg-token-already-used", new ClientError("User with this login is already registered!")},
                    {"inc-email", new ClientError("Incorrect email!")}
                }));
        }

        void ValidateLogin(string login)
        {
            if (string.IsNullOrEmpty(login))
                throw new ClientException("r-input-login");
            else if (login.Length > _configuration.MaxCountCharsLogin)
                throw new ClientException("too-long-login");
            else if (!Regex.Match(login, "^[a-zA-Z_0-9]*$").Success)
                throw new ClientException("login-spec-chars");
        }

        void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ClientException("r-input-name");
            else if (name.Length > _configuration.MaxCountCharsName)
                throw new ClientException("too-long-name");
            else if (!Regex.Match(name, "^[а-яА-ЯҐґЄєІіЇї]+$").Success)
                throw new ClientException("name-spec-chars");
        }

        void ValidateSurname(string surname)
        {
            if (string.IsNullOrEmpty(surname))
                throw new ClientException("r-input-surname");
            else if (surname.Length > _configuration.MaxCountCharsSurname)
                throw new ClientException("too-long-surname");
            else if (!Regex.Match(surname, "^[а-яА-ЯҐґЄєІіЇї]+$").Success)
                throw new ClientException("surname-spec-chars");
        }

        void ValidateLastname(string lastname)
        {
            if (string.IsNullOrEmpty(lastname))
                throw new ClientException("r-input-lastname");
            else if (lastname.Length > _configuration.MaxCountCharsLastname)
                throw new ClientException("too-long-lastname");
            else if (!Regex.Match(lastname, "^[а-яА-ЯҐґЄєІіЇї]+$").Success)
                throw new ClientException("lastname-spec-chars");
        }

        void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ClientException("r-input-pass");
            else if (password.Length > _configuration.MaxCountCharsPassword)
                throw new ClientException("too-long-pass");
            else if (password.Length < _configuration.MinCountCharsPassword)
                throw new ClientException("pass-count-chars");
            else if (!password.Any(c => char.IsDigit(c)))
                throw new ClientException("pass-count-digit");
        }

        void ValidateEmail(string email)
        {
            if (!Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                throw new ClientException("inc-email");
        }

        public string CreateRegistrationToken(TimeSpan? exp, string email, Role role, IEnumerable<Panel> noPanels = null, IEnumerable<Permission> noPermissions = null)
        {
            ValidateEmail(email);
            DateTime expires = DateTime.Now;
            if (!exp.HasValue)
                expires = expires.Add(_configuration.RegistrationTokenExpires);
            else expires = expires.Add(exp.Value);

            string nPanels = "";

            if (noPanels != null)
            {
                foreach (var item in noPanels)
                    nPanels += item.Id + " ";
                nPanels = nPanels.Substring(0, nPanels.Length - 1);
            }

            string nPermissions = "";

            if (noPermissions != null)
            {
                foreach (var item in noPermissions)
                    nPermissions += item.Id + " ";
                nPermissions = nPermissions.Substring(0, nPermissions.Length - 1);
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("email", email),
                new Claim("role", role.Id.ToString()),
                new Claim("noPanels", nPanels),
                new Claim("noPermissions", nPermissions)
            };

            var creds = new SigningCredentials(_configuration.RegistrationTokenValidation.IssuerSigningKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.RegistrationTokenValidation.ValidIssuer,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return _configuration.TokenHandler.WriteToken(token);
        }

        public string CreateRegistrationToken(TimeSpan? exp, string email, Role role)
        {
            return CreateRegistrationToken(exp, email, role, null);
        }

        public async Task<string> CreateRegistrationToken(TimeSpan? exp, string email, string role)
        {
            return CreateRegistrationToken(exp, email, await _roleService.Get(role));
        }

        public JwtSecurityToken ValidateRegistrationToken(string token)
        {
            SecurityToken validatedToken = null;
            try
            {
                _configuration.TokenHandler.ValidateToken(token, _configuration.RegistrationTokenValidation, out validatedToken);
            }
            catch (Exception)
            {
                throw new ClientException("inc-registration-token");
            }

            return validatedToken as JwtSecurityToken;
        }

        public void SendEmailCompleated(EmailSendCompleatedEntity entity)
        {
            var permanent = (PermanentSubscribeDto)entity.EmailEntity.AddtionalInfo;
            _notificationService.PermanentNotify(permanent.Token, "OnSendEmail", new OnSendEmailSource { Email = entity.EmailEntity.Message.To.FirstOrDefault()?.Address, Ok = entity.IsSended });
        }

        public PermanentSubscribeDto StartRegister(string email, Role role, IEnumerable<Panel> noPanels = null, IEnumerable<Permission> noPermissions = null)
        {
            var permanent = _notificationService.CreatePermanentToken(TimeSpan.FromHours(1));
            var regToken = CreateRegistrationToken(TimeSpan.FromDays(30), email, role, noPanels, noPermissions);
            _emailService.SendByDraft(email, "registration@schoolbridge.com", "Registration in Schoolbridge", "email-registration", SendEmailCompleated, permanent, regToken);
            return permanent;
        }

        public PermanentSubscribeDto StartRegister(string email, Role role)
        {
            return StartRegister(email, role, null);
        }

        public async Task<PermanentSubscribeDto> StartRegister(string email, string role)
        {
            return StartRegister(email, await _roleService.Get(role));
        }

        public async Task<User> EndRegister(EndRegisterDto entity)
        {
            var token = ValidateRegistrationToken(entity.RegistrationToken);

            if (await _userService.IsIsset(token.Id))
                throw new ClientException("reg-token-already-used");

            ValidateLogin(entity.Login);
            ValidatePassword(entity.Password);
            if (string.IsNullOrEmpty(entity.ConfirmPassword))
                throw new ClientException("input-repeat-pass");
            else if (entity.ConfirmPassword != entity.Password)
                throw new ClientException("inc-repeat-pass");
            ValidateName(entity.Name);
            ValidateSurname(entity.Surname);
            ValidateLastname(entity.Lastname);

            var user = new User
            {
                Id = token.Id,
                Login = entity.Login,
                Email = token.Claims.First(x => x.Type == "email").Value,
                Name = entity.Name,
                Surname = entity.Surname,
                Lastname = entity.Lastname,
                RoleId = int.Parse(token.Claims.First(x => x.Type == "role").Value),
                Role = new Role { Id = int.Parse(token.Claims.First(x => x.Type == "role").Value) },
                PasswordHash = PasswordHandler.CreatePasswordHash(entity.Password)
            };

            var noPanels = new List<Panel>();
            if (token.Claims.First(x => x.Type == "noPanels").Value.Length > 0) 
                foreach (var item in token.Claims.First(x => x.Type == "noPanels").Value.Split(' ')) 
                    noPanels.Add(new Panel { Id = int.Parse(item) });

            var noPermissions = new List<Permission>();
            if (token.Claims.First(x => x.Type == "noPermissions").Value.Length > 0)
                foreach (var item in token.Claims.First(x => x.Type == "noPermissions").Value.Split(' '))
                    noPermissions.Add(new Permission { Id = int.Parse(item) });

            return await _userService.Add(user, noPanels, noPermissions);
        }
    }
}

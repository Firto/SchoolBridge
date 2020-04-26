﻿using Microsoft.IdentityModel.Tokens;
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
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRoleService _roleService;
        private readonly IEmailService _emailService;
        private readonly INotificationService<User> _notificationService;
        private readonly IUserService _userService;
        private readonly IValidatingService _validatingService;
        private readonly ILoginService _loginService;
        private readonly RegistrationServiceConfiguration _configuration;
        public RegistrationService(IRoleService roleService,
                                    IEmailService emailService,
                                    INotificationService<User> notificationService,
                                    IUserService userService,
                                    IValidatingService validatingService,
                                    ILoginService loginService,
                                    RegistrationServiceConfiguration configuration,
                                    ClientErrorManager clientErrorManager)
        {
            _roleService = roleService;
            _emailService = emailService;
            _notificationService = notificationService;
            _userService = userService;
            _validatingService = validatingService;
            _loginService = loginService;
            _configuration = configuration;

            if (!clientErrorManager.IsIssetErrors("Registration"))
                clientErrorManager.AddErrors(new ClientErrors("Registration", new Dictionary<string, ClientError>
                {
                    {"r-token-inc", new ClientError("Incorrect registration token!")},
                    {"r-token-already-used", new ClientError("User with this registration token is already registered!")},
                }));
        }

        public string CreateRegistrationToken(TimeSpan exp, string email, Role role, IEnumerable<Panel> noPanels = null, IEnumerable<Permission> noPermissions = null)
        {
            _validatingService.ValidateEmail(email);
            DateTime expires = DateTime.Now.Add(exp);

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

        public string CreateRegistrationToken(TimeSpan exp, string email, Role role)
        {
            return CreateRegistrationToken(exp, email, role, null);
        }

        public async Task<string> CreateRegistrationToken(TimeSpan exp, string email, string role)
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
                throw new ClientException("r-token-inc");
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
            var regToken = CreateRegistrationToken(_configuration.RegistrationTokenExpires, email, role, noPanels, noPermissions);
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

        public async Task<LoggedDto> EndRegister(EndRegisterDto entity, string uuid)
        {
            var token = ValidateRegistrationToken(entity.RegistrationToken);

            if (await _userService.IsIssetAsync(token.Id))
                throw new ClientException("r-token-already-used");

            _validatingService.ValidateLogin(entity.Login);
            _validatingService.ValidatePassword(entity.Password);
            _validatingService.ValidateRepeatPassword(entity.Password, entity.ConfirmPassword);
            _validatingService.ValidateName(entity.Name);
            _validatingService.ValidateSurname(entity.Surname);
            _validatingService.ValidateLastname(entity.Lastname);

            var user = new User
            {
                Id = token.Id,
                Login = entity.Login,
                Email = token.Claims.First(x => x.Type == "email").Value,
                Name = entity.Name,
                Surname = entity.Surname,
                Lastname = entity.Lastname,
                RoleId = int.Parse(token.Claims.First(x => x.Type == "role").Value),
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
            var usr = await _userService.Add(user, noPanels, noPermissions);
            usr.Role = await _roleService.Get(usr.RoleId);
            return await _loginService.Login(usr, uuid);
        }
    }
}

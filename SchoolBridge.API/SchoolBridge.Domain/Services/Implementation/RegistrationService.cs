using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.AddtionalClases.EmailService;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;
using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.DtoModels.Authefication;
using SchoolBridge.Helpers.Extentions;
using SchoolBridge.Helpers.Managers;
using SchoolBridge.Domain.Managers.CClientErrorManager;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRoleService _roleService;
        private readonly IEmailService _emailService;
        private readonly IPermanentConnectionService _permanentConnectionService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly HttpContext _httpContext;
        private readonly RegistrationServiceConfiguration _configuration;

        public RegistrationService(IRoleService roleService,
                                    IEmailService emailService,
                                    INotificationService notificationService,
                                    IPermanentConnectionService permanentConnectionService,
                                    IUserService userService,
                                    ILoginService loginService,
                                    ScopedHttpContext httpContext,
                                    RegistrationServiceConfiguration configuration)
        {
            _roleService = roleService;
            _emailService = emailService;
            _notificationService = notificationService;
            _permanentConnectionService = permanentConnectionService;
            _userService = userService;
            _loginService = loginService;
            _httpContext = httpContext.HttpContext;
            _configuration = configuration;
        }

        public static void OnInit(ClientErrorManager manager)
        {
            manager.AddErrors(new ClientErrors("Registration", new Dictionary<string, ClientError>
                {
                    { "r-token-inc", new ClientError("Incorrect registration token!")},
                    { "r-token-already-used", new ClientError("User with this registration token is already registered!")},
                    { "r-login-alrd-reg", new ClientError("User with this login is already registered!") },
                    { "r-email-alrd-reg", new ClientError("User with this email is already registered!")},
                    { "r-ch-pass-token-inc", new ClientError("Change password token incorrect") },
                    { "r-email-no-reg", new ClientError("Change password token incorrect")}
                }));
        }

        public string CreateRegistrationToken(TimeSpan exp, string email, Role role, IEnumerable<Permission> noPermissions = null)
        {
            DateTime expires = DateTime.Now.Add(exp);

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

        public string CreateChnagePasswordToken(TimeSpan exp, string email, string oldPassword)
        {
            DateTime expires = DateTime.Now.Add(exp);

            var claims = new List<Claim>
            {
                new Claim("email", email),
                new Claim("oldPassword", oldPassword)
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
            return CreateRegistrationToken(exp, email, await _roleService.GetAsync(role));
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
            var tokenId = (string)entity.EmailEntity.AddtionalInfo;
            _notificationService.PermanentNotifyAsync(tokenId, "onSendEmail", new OnSendEmailSource { Ok = entity.IsSended, Email = entity.EmailEntity.Message.To.FirstOrDefault()?.Address });
        }
        public PermanentSubscribeDto StartRegister(string email, Role role, IEnumerable<Permission> noPermissions = null)
        {
            var permanent = _permanentConnectionService.CreatePermanentToken(TimeSpan.FromHours(1), out var permTokenId);
            var regToken = CreateRegistrationToken(_configuration.RegistrationTokenExpires, email, _roleService.Get(role.Name), noPermissions);
            _emailService.SendByDraft(email,
                                        "registration@schoolbridge.com",
                                        "Registration in Schoolbridge",
                                        "email-registration",
                                        SendEmailCompleated,
                                        EmailEntityPriority.High,
                                        permTokenId,
                                        string.Format("{0}:{1}/start/endregister?token={2}", _httpContext.Connection.RemoteIpAddress.ToString(), 4200, HttpUtility.UrlEncode(regToken)));
            return permanent;
        }

        public PermanentSubscribeDto StartRegister(string email, Role role)
        {
            return StartRegister(email, role, null);
        }

        public async Task<PermanentSubscribeDto> StartRegister(string email, string role)
        {
            return StartRegister(email, await _roleService.GetAsync(role));
        }

        public async Task<LoggedDto> EndRegister(EndRegisterDto entity, string uuid)
        {
            var token = ValidateRegistrationToken(entity.RegistrationToken);

            if (await _userService.IsIssetAsync(token.Id))
                throw new ClientException("r-token-already-used");
            else if (_userService.IsIssetByLogin(entity.Login))
                throw new ClientException("r-login-alrd-reg");
            else if (_userService.IsIssetByEmail(token.Claims.First(x => x.Type == "email").Value))
                throw new ClientException("r-email-alrd-reg");

            var user = new User
            {
                Id = token.Id,
                Login = entity.Login,
                Email = token.Claims.First(x => x.Type == "email").Value,
                Name = entity.Name,
                Surname = entity.Surname,
                Lastname = entity.Lastname,
                PhotoId = "default-user-photo",
                Birthday = entity.Birthday.Value,
                RoleId = int.Parse(token.Claims.First(x => x.Type == "role").Value),
                PasswordHash = PasswordHandler.CreatePasswordHash(entity.Password)
            };

            var noPermissions = new List<Permission>();
            if (token.Claims.First(x => x.Type == "noPermissions").Value.Length > 0)
                foreach (var item in token.Claims.First(x => x.Type == "noPermissions").Value.Split(' '))
                    noPermissions.Add(new Permission { Id = int.Parse(item) });
            var usr = await _userService.AddAsync(user, noPermissions);
            usr.Role = await _roleService.GetAsync(usr.RoleId);
            return await _loginService.Login(usr, uuid);
        }

        public PermanentSubscribeDto ChangePasswordEmail(string email)
        {
            if (!_userService.IsIssetByEmail(email))
                throw new ClientException("r-email-no-reg");

            var permanent = _permanentConnectionService.CreatePermanentToken(TimeSpan.FromHours(1), out var permTokenId);
            var regToken = CreateChnagePasswordToken(_configuration.ChangePasswordTokenExpires, email, _userService.GetByEmail(email).PasswordHash);
            _emailService.SendByDraft(email,
                                        "registration@schoolbridge.com",
                                        "Change password in Schoolbridge",
                                        "changing-password",
                                        SendEmailCompleated,
                                        EmailEntityPriority.High,
                                        permTokenId,
                                        regToken);
            return permanent;
        }

        public async Task<LoggedDto> EndChangePasswordEmail(EndChangePasswordEmailDto entity, string uuid)
        {
            var token = ValidateRegistrationToken(entity.changePasswordToken);

            var user = await _userService.GetByEmailAsync(token.Claims.First(x => x.Type == "email").Value);

            if (user == null)
                throw new ClientException("r-email-no-reg");
           
            if (user.PasswordHash != token.Claims.First(x => x.Type == "oldPassword").Value)
                throw new ClientException("r-ch-pass-token-inc");

            await _userService.SetPasswordAsync(user, entity.newPassword);

            return await _loginService.Login(_userService.GetAll(user.Id), uuid);
        }
    }
}
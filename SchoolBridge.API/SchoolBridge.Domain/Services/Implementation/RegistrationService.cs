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
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;
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
        private readonly INotificationService<User> _notificationService;
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly HttpContext _httpContext;
        private readonly RegistrationServiceConfiguration _configuration;

        public RegistrationService(IRoleService roleService,
                                    IEmailService emailService,
                                    INotificationService<User> notificationService,
                                    IUserService userService,
                                    IValidatingService validatingService,
                                    ILoginService loginService,
                                    ScopedHttpContext httpContext,
                                    RegistrationServiceConfiguration configuration,
                                    ClientErrorManager clientErrorManager)
        {
            _roleService = roleService;
            _emailService = emailService;
            _notificationService = notificationService;
            _userService = userService;
            _loginService = loginService;
            _httpContext = httpContext.HttpContext;
            _configuration = configuration;

            if (!clientErrorManager.IsIssetErrors("Registration"))
            {
                clientErrorManager.AddErrors(new ClientErrors("Registration", new Dictionary<string, ClientError>
                {
                    { "r-token-inc", new ClientError("Incorrect registration token!")},
                    { "r-token-already-used", new ClientError("User with this registration token is already registered!")},
                    { "r-login-alrd-reg", new ClientError("User with this login is already registered!") },
                    { "r-email-alrd-reg", new ClientError("User with this email is already registered!")}
                }));

                // validation //  [PropValid("str-input", "str-email", "str-email-no-reg")]

                validatingService.AddValidateFunc("date-birthday", (DateTime? prop, PropValidateContext context) => {
                    if (prop == null) return;

                    if (prop.Value == null || prop.Value > DateTime.Now)
                        context.Valid.Add($"[r-date-birthday-incorrect]"); 
                });

                validatingService.AddValidateFunc("str-email-reg-no", (string prop, PropValidateContext context) => {
                    if (prop == null) return;

                    if (context.SeriviceProvider.GetService<IUserService>().IsIssetByEmail(prop))
                        context.Valid.Add($"[r-email-reg]"); // "User with this email is already registered!"
                });

                validatingService.AddValidateFunc("str-login", (string prop, PropValidateContext ctx) => {
                    if (prop == null) return;

                    if (prop.Length > _configuration.MaxCountCharsLogin)
                        ctx.Valid.Add($"[str-too-long, [pn-{ctx.PropName}], {_configuration.MaxCountCharsLogin}]"); // "Too long login(max {_configuration.MaxCountCharsLogin} characters)!"
                    if (!Regex.Match(prop, "^[a-zA-Z_0-9]*$").Success)
                        ctx.Valid.Add($"[str-no-spc-ch, [pn-{ctx.PropName}]]"); // "Login musn't have specials chars!"
                });

                validatingService.AddValidateFunc("str-creds", (string prop, PropValidateContext ctx) => {
                    if (prop == null) return;

                    if (prop.Length > _configuration.MaxCountCharsName)
                        ctx.Valid.Add($"[str-too-long, [pn-{ctx.PropName}], {255}]");
                    if (!Regex.Match(prop, "^[à-ÿÀ-ß¥´ªº²³¯¿]+$").Success)
                        ctx.Valid.Add($"[str-no-spc-ch-2, [pn-{ctx.PropName}]]"); //"Name musn't have specials chars!"
                });

                validatingService.AddValidateFunc("str-password", (string prop, PropValidateContext ctx) => {
                    if (prop == null) return;

                    if (prop.Length > _configuration.MaxCountCharsPassword)
                        ctx.Valid.Add($"[str-too-long, [pn-{ctx.PropName}], {_configuration.MaxCountCharsPassword}]");//$"Too long password(max{} characters)!"
                    if (prop.Length < _configuration.MinCountCharsPassword)
                        ctx.Valid.Add($"[str-too-sh, [pn-{ctx.PropName}], {_configuration.MaxCountCharsPassword}]");//$"Password must have {_configuration.MinCountCharsPassword} and more chars!
                    if (!prop.Any(c => char.IsDigit(c)))
                        ctx.Valid.Add($"[str-no-dig, [pn-{ctx.PropName}]]");//$"Password must have minimum one digit!"
                });

                validatingService.AddValidateFunc("str-password-rep", (string prop, PropValidateContext ctx) => {
                    if (prop == null || ctx.TypeDto.GetProperty("Password") == null ||
                                        ctx.TypeDto.GetProperty("Password").GetValue(ctx.Dto) == null) return;


                    if (prop != (string)ctx.TypeDto.GetProperty("Password").GetValue(ctx.Dto))
                        ctx.Valid.Add($"[str-inc-rep, [pn-{ctx.PropName}]]"); //$"Incorrect repeat password!"
          
                });
            }
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
            var tokenId = (string)entity.EmailEntity.AddtionalInfo;
            _notificationService.PermanentNotify(tokenId, "onSendEmail", new OnSendEmailSource { Ok = entity.IsSended, Email = entity.EmailEntity.Message.To.FirstOrDefault()?.Address });
        }
        public PermanentSubscribeDto StartRegister(string email, Role role, IEnumerable<Permission> noPermissions = null)
        {
            var permanent = _notificationService.CreatePermanentToken(TimeSpan.FromHours(1), out var permTokenId);
            var regToken = CreateRegistrationToken(_configuration.RegistrationTokenExpires, email, role, noPermissions);
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
            return StartRegister(email, await _roleService.Get(role));
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
                Birthday = entity.Birtday.Value,
                RoleId = int.Parse(token.Claims.First(x => x.Type == "role").Value),
                PasswordHash = PasswordHandler.CreatePasswordHash(entity.Password)
            };

            var noPermissions = new List<Permission>();
            if (token.Claims.First(x => x.Type == "noPermissions").Value.Length > 0)
                foreach (var item in token.Claims.First(x => x.Type == "noPermissions").Value.Split(' '))
                    noPermissions.Add(new Permission { Id = int.Parse(item) });
            var usr = await _userService.AddAsync(user, noPermissions);
            usr.Role = await _roleService.Get(usr.RoleId);
            return await _loginService.Login(usr, uuid);
        }
    }
}
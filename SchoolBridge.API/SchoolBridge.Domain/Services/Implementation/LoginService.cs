using AutoMapper;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.AddtionalClases.DataBaseNotoficationService;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.Managers;
using SchoolBridge.Domain.Managers.CClientErrorManager;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;
using SchoolBridge.Helpers.Managers.Image;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SchoolBridge.Helpers.DtoModels.Authefication;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;
using System;
using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using SchoolBridge.Domain.Services.Configuration;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class LoginService : ILoginService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IDataBaseNotificationService _dataBaseNotificationService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        

        public LoginService(    IUserService userService,
                                ITokenService tokenService,
                                IDataBaseNotificationService dataBaseNotificationService,
                                IImageService imageService,
                                IMapper mapper)
        {
            _userService = userService;
            _tokenService = tokenService;
            _dataBaseNotificationService = dataBaseNotificationService;
            _imageService = imageService;
            _mapper = mapper;
        }

        public static void OnInit(ClientErrorManager manager, IValidatingService validatingService, RegistrationServiceConfiguration configuration)
        {
            manager.AddErrors(new ClientErrors("Login", new Dictionary<string, ClientError>(){
                    {"l-user-banned", new ClientError("User banned!")},
                    {"l-pass-log-inc", new ClientError("Incorrect login or password!")},
                    {"l-too-many-devices", new ClientError("Too many devices logged!")}
                }));

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

                if (prop.Length > configuration.MaxCountCharsLogin)
                    ctx.Valid.Add($"[str-too-long, [pn-{ctx.PropName}], {configuration.MaxCountCharsLogin}]"); // "Too long login(max {_configuration.MaxCountCharsLogin} characters)!"
                if (!Regex.Match(prop, "^[a-zA-Z_0-9]*$").Success)
                    ctx.Valid.Add($"[str-no-spc-ch, [pn-{ctx.PropName}]]"); // "Login musn't have specials chars!"
            });

            validatingService.AddValidateFunc("str-creds", (string prop, PropValidateContext ctx) => {
                if (prop == null) return;

                if (prop.Length > configuration.MaxCountCharsName)
                    ctx.Valid.Add($"[str-too-long, [pn-{ctx.PropName}], {255}]");
                if (!Regex.Match(prop, "^[а-яА-ЯҐґЄєІіЇї]+$").Success)
                    ctx.Valid.Add($"[str-no-spc-ch-2, [pn-{ctx.PropName}]]"); //"Name musn't have specials chars!"
            });

            validatingService.AddValidateFunc("str-password", (string prop, PropValidateContext ctx) => {
                if (prop == null) return;

                if (prop.Length > configuration.MaxCountCharsPassword)
                    ctx.Valid.Add($"[str-too-long, [pn-{ctx.PropName}], {configuration.MaxCountCharsPassword}]");//$"Too long password(max{} characters)!"
                if (prop.Length < configuration.MinCountCharsPassword)
                    ctx.Valid.Add($"[str-too-sh, [pn-{ctx.PropName}], {configuration.MaxCountCharsPassword}]");//$"Password must have {_configuration.MinCountCharsPassword} and more chars!
                if (!prop.Any(c => char.IsDigit(c)))
                    ctx.Valid.Add($"[str-no-dig, [pn-{ctx.PropName}]]");//$"Password must have minimum one digit!"
            });

            validatingService.AddValidateFunc("str-password-rep", (string prop, PropValidateContext ctx) => {
                if (prop == null || ctx.TypeDto.GetProperty("Password") == null ||
                                    ctx.TypeDto.GetProperty("Password").GetValue(ctx.Dto) == null) return;


                if (prop != (string)ctx.TypeDto.GetProperty("Password").GetValue(ctx.Dto))
                    ctx.Valid.Add($"[str-inc-rep, [pn-{ctx.PropName}]]"); //$"Incorrect repeat password!"

            });

            validatingService.AddValidateFunc("str-email", (string prop, PropValidateContext context) =>
            {
                if (prop == null) return;

                if (!Regex.IsMatch(prop, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                    context.Valid.Add($"[v-str-email, [pn-{context.PropName}]]");
            });
        }

        public async Task<LoggedDto> Login(User user, string uuid) {
            await _dataBaseNotificationService.NotifyNoBaseAsync(user.Id, "onLogin", new MessageNotificationSource { Message = "Login from another device!" });
            var dto = new LoggedDto
            {
                Tokens = await _tokenService.Login(user.Id, uuid),
                Permissions = user.Permissions.Select(x => x.Permission.Name),
                Role = user.Role.Name,
                UserId = user.Id,
                CountUnreadNotifications = await _dataBaseNotificationService.GetCountUnreadAsync(user.Id)
            };
            return dto;
        }

        public async Task<LoggedDto> Login(LoginDto entity, string uuid)
        {
            var user = _userService.GetAllByLogin(entity.Login);
            if (user == null || !PasswordHandler.Validate(entity.Password, user.PasswordHash))
                throw new ClientException("l-pass-log-inc");
            else if (user.Banned != null)
                throw new ClientException("l-user-banned", user.Banned);
            else if (_tokenService.CountLoggedDevices(user.Id) > 10)
                throw new ClientException("l-too-many-devices");

            return await Login(user, uuid);
        }

        public async Task<LoggedTokensDto> RefreshToken(RefreshTokenDto entity, string uuid)
            => await _tokenService.RefreshToken(entity.RefreshToken, uuid);

        public async Task Logout(IHeaderDictionary headers)
            => await _tokenService.DeactivateToken(headers);

        public async Task LogoutAll(IHeaderDictionary headers)
            => await _tokenService.DeactivateAllTokens(headers);

        public Task ChangePasswordEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public Task<LoggedDto> EndChangePasswordEmail(EndChangePasswordEmailDto entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
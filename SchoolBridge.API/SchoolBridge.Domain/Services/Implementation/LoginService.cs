using AutoMapper;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.AddtionalClases.DataBaseNotoficationService;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.Managers;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;
using SchoolBridge.Helpers.Managers.Image;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SchoolBridge.Helpers.DtoModels.Authefication;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class LoginService : ILoginService
    {
        private readonly IUserService _userService;
        private readonly ITokenService<User> _tokenService;
        private readonly IDataBaseNotificationService<User> _dataBaseNotificationService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        

        public LoginService(  ClientErrorManager clientErrorManager,
                                IUserService userService,
                                ITokenService<User> tokenService,
                                IDataBaseNotificationService<User> dataBaseNotificationService,
                                IImageService imageService,
                                IValidatingService validatingService,
                                IMapper mapper)
        {
            _userService = userService;
            _tokenService = tokenService;
            _dataBaseNotificationService = dataBaseNotificationService;
            _imageService = imageService;
            _mapper = mapper;

            if (!clientErrorManager.IsIssetErrors("Login"))
            {
                clientErrorManager.AddErrors(new ClientErrors("Login", new Dictionary<string, ClientError>(){
                    {"l-pass-log-inc", new ClientError("Incorrect login or password!")},
                    {"l-too-many-devices", new ClientError("Too many devices logged!")}
                }));

                validatingService.AddValidateFunc<LoginDto>((IValidatingService ser, object obj) => {
                    IDictionary<string, string> valid = new Dictionary<string, string>();
                    LoginDto entity = (LoginDto)obj;

                    if (string.IsNullOrEmpty(entity.Login))
                        valid.Add(nameof(entity.Login), "Please input login!");
                    if (string.IsNullOrEmpty(entity.Password))
                        valid.Add(nameof(entity.Password), "Please input password!");

                    return valid;
                });
            }
        }

        public async Task<LoggedDto> Login(User user, string uuid) {
            await _dataBaseNotificationService.Notify(user, "onLogin", new MessageNotificationSource { Message = "Login from another device!" });
            var dto = new LoggedDto
            {
                Tokens = await _tokenService.Login(user, uuid),
                Panels = user.Panels.Select(x => x.Panel.Name),
                Permissions = user.Permissions.Select(x => x.Permission.Name),
                Role = user.Role.Name
            };
            if (user.Notifications == null)
                dto.Notifications = await _dataBaseNotificationService.Get(user, null);
            else dto.Notifications = _mapper.Map<IEnumerable<Notification<User>>, IEnumerable<DataBaseSourse>>(user.Notifications.OrderByDescending((x) => x.Date).Take(20).ToArray());
            return dto;
        }

        public async Task<LoggedDto> Login(LoginDto entity, string uuid)
        {
            var user = _userService.GetAllByLogin(entity.Login);
            if (user == null || !PasswordHandler.Validate(entity.Password, user.PasswordHash))
                throw new ClientException("l-pass-log-inc");
            else if (_tokenService.CountLoggedDevices(user) > 10)
                throw new ClientException("l-too-many-devices");

            return await Login(user, uuid);
        }

        public async Task<LoggedTokensDto> RefreshToken(RefreshTokenDto entity, string uuid)
            => await _tokenService.RefreshToken(entity.RefreshToken, uuid);

        public async Task Logout(IHeaderDictionary headers)
            => await _tokenService.DeactivateToken(headers);

        public async Task LogoutAll(IHeaderDictionary headers)
            => await _tokenService.DeactivateAllTokens(headers);
    }
}
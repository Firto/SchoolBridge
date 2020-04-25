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

namespace SchoolBridge.Domain.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<User> _usersGR;
        private readonly ITokenService<User> _tokenService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IDataBaseNotificationService<User> _dataBaseNotificationService;

        public AccountService(  ClientErrorManager clientErrorManager,
                                IGenericRepository<User> usersGR,
                                ITokenService<User> tokenService,
                                IImageService imageService,
                                IMapper mapper,
                                IDataBaseNotificationService<User> dataBaseNotificationService)
        {
            _usersGR = usersGR;
            _tokenService = tokenService;
            _imageService = imageService;
            _mapper = mapper;
            _dataBaseNotificationService = dataBaseNotificationService;

            if (!clientErrorManager.IsIssetErrors("Account"))
                clientErrorManager.AddErrors(new ClientErrors("Account",
                    new Dictionary<string, ClientError>
                    {
                        {"no-login", new ClientError("You are not logged in!")},
                        {"already-login", new ClientError("You are already logged in!")},
                        {"base-account-err", new ClientError("Error in account service!")}
                    },
                    new List<ClientErrors>(){
                        new ClientErrors("Login", new Dictionary<string, ClientError>()
                        {
                            {"input-login", new ClientError("Please input login!")},
                            {"input-password", new ClientError("You are already logged in!")},
                            {"inc-log-pass", new ClientError("Incorrect login or password!")},
                            {"too-many-devices", new ClientError("Too many devices logged!")}
                        }),
                        new ClientErrors("Edit", new Dictionary<string, ClientError>(){
                            {"inc-email", new ClientError("Incorrect email!")},
                            {"alrd-reg-login", new ClientError("User with this login is already registered!")},
                            {"alrd-reg-email", new ClientError("User with this email is already registered!")},
                            {"inc-profile-image", new ClientError("Incorrect profile image!")}
                        })
                    }
                ));
        }

        public async Task<LoggedDto> Login(LoginDto entity, string uuid)
        {
            if (string.IsNullOrEmpty(entity.Login))
                throw new ClientException("input-login");
            else if (string.IsNullOrEmpty(entity.Password))
                throw new ClientException("input-password");

            var users = await _usersGR.GetAllAsync((x) => x.Login == entity.Login);
            if (users.Count() != 1 || !PasswordHandler.Validate(entity.Password, users.First().PasswordHash))
                throw new ClientException("inc-log-pass");

            if (_tokenService.CountLoggedDevices(users.First()) > 10)
                throw new ClientException("too-many-devices");

            await _dataBaseNotificationService.Notify(users.First(), "onLogin", new MessageNotificationSource { Message = "Login from another device!" });
            return await _tokenService.Login(users.First(), uuid);
        }

        /*public async Task<LoggedDto> Register(RegisterDto entity, string uuid)
        {
            ValidateLogin(entity.Login);
            ValidatePassword(entity.Password);

            if (string.IsNullOrEmpty(entity.ConfirmPassword))
                throw new ClientException("input-repeat-pass");
            else if (entity.ConfirmPassword != entity.Password)
                throw new ClientException("inc-repeat-pass");

            var user = new User
            {
                Login = entity.Login,
                PhotoId = "default-profile-photo",
                RoleId = 1,
                PasswordHash = PasswordHandler.CreatePasswordHash(entity.Password)
            };

            if (_usersGR.CountWhere((x) => x.Login == entity.Login) > 0)
                throw new ClientException("already-registered");

            await _usersGR.CreateAsync(user);
            return await Login(new LoginDto { Login = entity.Login, Password = entity.Password }, uuid);
        }*/

        public async Task<LoggedDto> RefreshToken(RefreshTokenDto entity, string uuid)
            => await _tokenService.RefreshToken(entity.RefreshToken, uuid);

        public async Task<ProfileDto> GetProfileInfo(User user)
            => _mapper.Map<User, ProfileDto>(await _usersGR.FindAsync(user.Id));

        /*public async Task<ProfileDto> SetProfileInfo(User user, ProfileDto entity)
        {
            string remPhotoId = null;
            user = _usersGR.Find(user.Id);
            if (entity.Login != null && entity.Login != user.Login)
            {
                ValidateLogin(entity.Login);

                if (await _usersGR.CountWhereAsync((x) => x.Login == entity.Login) > 0)
                    throw new ClientException("alrd-reg-login");

                user.Login = entity.Login;
            }

            if (entity.Email != null && entity.Email != user.Email)
            {
                ValidateEmail(entity.Email);
                if (await _usersGR.CountWhereAsync((x) => x.Email == entity.Email) > 0)
                    throw new ClientException("alrd-reg-email");
                user.Email = entity.Email;
            }

            //if (entity.Photo != null)
            //{
            //    remPhotoId = user.PhotoId;
            //    user.PhotoId = await _imageService.Add(DataImage.TryParse(entity.Photo));
            //}

            await _usersGR.UpdateAsync(user);
            if (remPhotoId != null) await _imageService.Remove(remPhotoId);
            return await GetProfileInfo(await _usersGR.FindAsync(user.Id));
        }*/

        public async Task Logout(IHeaderDictionary headers)
            => await _tokenService.DeactivateToken(headers);

        public async Task LogoutAll(IHeaderDictionary headers)
            => await _tokenService.DeactivateAllTokens(headers);
    }
}
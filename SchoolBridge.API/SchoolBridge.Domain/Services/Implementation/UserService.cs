using Microsoft.EntityFrameworkCore;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.Extentions;
using SchoolBridge.Helpers.Managers;
using SchoolBridge.Domain.Managers.CClientErrorManager;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using SchoolBridge.Domain.Services.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userGR;
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;

        private readonly IGenericRepository<DefaultRolePermission> _defaultRolePermissionGR;
        private readonly IOnlineService _onlineService;
        private readonly IImageService _imageService;
        private readonly UserServiceConfiguration _configuration;
        public UserService(IGenericRepository<User> userGR,
                            IPermissionService permissionService,
                            IRoleService roleService,
                            IGenericRepository<DefaultRolePermission> defaultRolePermissionGR,
                            IOnlineService onlineService,
                            UserServiceConfiguration configuration,
                            IImageService imageService)
        {
            _userGR = userGR;
            _permissionService = permissionService;
            _roleService = roleService;
            _defaultRolePermissionGR = defaultRolePermissionGR;
            _onlineService = onlineService;
            _configuration = configuration;
            _imageService = imageService;
        }

        public static void OnInit(ClientErrorManager manager)
        {
            manager.AddErrors(new ClientErrors("User", new Dictionary<string, ClientError>
                {
                    { "u-inc-user-id" , new ClientError ("Incorrect user id!")},
                    { "u-inc-gettoken" , new ClientError ("Incorrect get user token!")}
                }));
        }

        public static void OnFirstInit(IGenericRepository<User> userGR, IGenericRepository<Role> roleGR)
        {
            userGR.Create(new User
            {
                Id = "admin",
                Name = "Admin",
                Surname = "Admin",
                Lastname = "Admin",
                Email = "admin@admin.admin",
                Login = "admin",
                PhotoId = "default-user-photo",
                PasswordHash = PasswordHandler.CreatePasswordHash("admin"),
                RoleId = roleGR.GetAll(x => x.Name == "Admin").FirstOrDefault().Id
            });
        }

        public UserGetToken CreateGetToken(string userId, TimeSpan expires)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, userId),
            };

            var creds = new SigningCredentials(_configuration.UserGetTokenValidation.IssuerSigningKey, SecurityAlgorithms.HmacSha256);
            var expiress = DateTime.Now.Add(expires);
            var token = new JwtSecurityToken(
                issuer: _configuration.UserGetTokenValidation.ValidIssuer,
                expires: expiress,
                claims: claims,
                signingCredentials: creds
            );

            return new UserGetToken { Token = _configuration.TokenHandler.WriteToken(token), Expires = expiress.ToUnixTimestamp() };
        }

        public UserGetToken CreateStaticGetToken(string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, userId),
            };
            var creds = new SigningCredentials(_configuration.UserGetTokenValidation.IssuerSigningKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration.UserGetTokenValidation.ValidIssuer,
                expires: DateTime.UnixEpoch,
                claims: claims,
                signingCredentials: creds
            );

            return new UserGetToken { Token = _configuration.TokenHandler.WriteToken(token), Expires = 0 };
        }

        public JwtSecurityToken ValidateToken(string token)
        {
            SecurityToken validatedToken;
            try
            {
                _configuration.TokenHandler.ValidateToken(token, _configuration.UserGetTokenValidation, out validatedToken);
            }
            catch (SecurityTokenExpiredException ex)
            {
                if (ex.Expires != DateTime.UnixEpoch)
                    throw new ClientException("u-inc-gettoken");
                return new JwtSecurityToken(token);
            }
            catch (Exception) {
                throw new ClientException("u-inc-gettoken");
            }

            return validatedToken as JwtSecurityToken;
        }

        public UserDto GetFullDto(string tokenId, string Id) {
            var usr = _userGR.GetDbSet()
                          .Where(x => x.Id == Id)
                          .Include(x => x.Role).FirstOrDefault();
            return new UserDto
            {
                Id = usr.Id,
                Login = usr.Login,
                Role = usr.Role.Name,
                Photo = usr.PhotoId,
                OnlineStatusSubscriptionToken = _onlineService.CreateOnlineStatusSubscriptionToken(tokenId, Id),
                OnlineStatus = _onlineService.GetOnlineStatus(Id)
            };
        }

        public UserDto GetFullDtoByGetToken(string tokenId, string getToken)
        {
            var token = ValidateToken(getToken);
            return GetFullDto(tokenId, token.Id);
        }

        public ShortUserDto GetShortDto(string Id)
        {
            return new ShortUserDto
            {
                Id = Id,
                GetToken = CreateGetToken(Id, new TimeSpan(0, 15, 0))
            };
        }

        public ShortUserDto GetStaticShortDto(string Id) {
            return new ShortUserDto
            {
                Id = Id,
                GetToken = CreateStaticGetToken(Id)
            };
        }

        public async Task<UserDto> GetFullDtoAsync(string tokenId, string Id)
        {
            var usr = await _userGR.GetDbSet()
                          .Where(x => x.Id == Id)
                          .Include(x => x.Role).FirstOrDefaultAsync();
            return new UserDto
            {
                Id = usr.Id,
                Login = usr.Login,
                Role = usr.Role.Name,
                Photo = usr.PhotoId,
                OnlineStatusSubscriptionToken = _onlineService.CreateOnlineStatusSubscriptionToken(tokenId, Id),
                OnlineStatus = _onlineService.GetOnlineStatus(Id)
            };
        }

        public async Task<UserDto> GetFullDtoByGetTokenAsync(string tokenId, string getToken)
        {
            var token = ValidateToken(getToken);
            return await GetFullDtoAsync(tokenId, token.Id);
        }

        public async Task<User> AddAsync(User user, IEnumerable<Permission> noPermissions = null)
        {
            user = await _userGR.CreateAsync(user);
            var permissions = _defaultRolePermissionGR.GetDbSet()
                .Where(x => x.RoleId == user.RoleId)
                .Include(x => x.Permission)
                .ToArray()
                .TakeWhile(x => noPermissions.FirstOrDefault(z => z.Id == x.PermissionId) == null)
                .Select(x => x.Permission);
         
            user.Permissions = await _permissionService.AddUserPermissionsAsync(user, permissions);
            return user;
        }

        public async Task<User> AddAsync(User user, string role, IEnumerable<Permission> noPermissions = null)
        {
            user.Role = await _roleService.GetAsync(role);
            return await AddAsync(user, noPermissions);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return (await _userGR.GetAllAsync(x => x.Email == email)).FirstOrDefault();
        }

        public async Task<bool> IsIssetByEmailAsync(string email)
        {
            return (await _userGR.CountWhereAsync(x => x.Email == email)) > 0;
        }

        public async Task<User> GetByLoginAsync(string login)
        {
            return (await _userGR.GetAllAsync(x => x.Login == login)).FirstOrDefault();
        }

        public async Task<bool> IsIssetByLoginAsync(string login)
        {
            return (await _userGR.CountWhereAsync(x => x.Login == login)) > 0;
        }

        public async Task<User> GetAsync(string Id)
        {
            var usr = await _userGR.FindAsync(Id);
            if (usr == null)
                throw new ClientException("u-inc-user-id");
            return usr;
        }

        public async Task<bool> IsIssetAsync(string Id)
        {
            return (await _userGR.FindAsync(Id)) != null;
        }

        public User GetByEmail(string email)
        {
            return _userGR.GetAll(x => x.Email == email).FirstOrDefault();
        }

        public bool IsIssetByEmail(string email)
        {
            return _userGR.CountWhere(x => x.Email == email) > 0;
        }

        public User GetByLogin(string login)
        {
            return _userGR.GetAll(x => x.Login == login).FirstOrDefault();
        }

        public bool IsIssetByLogin(string login)
        {
            return _userGR.CountWhere(x => x.Login == login) > 0;
        }

        public User Get(string Id)
        {
            var usr = _userGR.Find(Id);
            if (usr == null)
                throw new ClientException("u-inc-user-id");
            return usr;
        }

        public bool IsIsset(string Id)
        {
            return Get(Id) != null;
        }

        public async Task RemoveAsync(User user)
        {
            await _userGR.DeleteAsync(user);
        }


        public User GetAll(string Id)
        {
            return _userGR.GetDbSet()
                          .Where(x => x.Id == Id)
                          .Include(x => x.Role)
                          .Include(x => x.Permissions)
                          .ThenInclude(x => x.Permission)
                          .Include(x => x.Notifications)
                          .ToArray()
                          .FirstOrDefault();
        }

        public User GetAllByEmail(string email)
        {
            return _userGR.GetDbSet()
                          .Where(x => x.Email == email)
                          .Include(x => x.Role)
                          .Include(x => x.Permissions)
                          .ThenInclude(x => x.Permission)
                          .Include(x => x.Notifications)
                          .ToArray()
                          .FirstOrDefault();
        }

        public User GetAllByLogin(string login)
        {
            return _userGR.GetDbSet()
                          .Where(x => x.Login == login)
                          .Include(x => x.Role)
                          .Include(x => x.Permissions)
                          .ThenInclude(x => x.Permission)
                          .Include(x => x.Notifications)
                          .ToArray()
                          .FirstOrDefault();
        }

        public async Task<User> GetAllAsync(string Id)
        {
            return (await _userGR.GetDbSet()
                                 .Where(x => x.Id == Id)
                                 .Include(x => x.Role)
                                 .Include(x => x.Permissions)
                                 .ThenInclude(x => x.Permission)
                                 .Include(x => x.Notifications)
                                 .ToArrayAsync()).FirstOrDefault();
        }

        public async Task<User> GetAllByEmailAsync(string email)
        {
            return (await _userGR.GetDbSet()
                                 .Where(x => x.Email == email)
                                 .Include(x => x.Role)
                                 .Include(x => x.Permissions)
                                 .ThenInclude(x => x.Permission)
                                 .Include(x => x.Notifications)
                                 .ToArrayAsync()).FirstOrDefault();
        }

        public async Task<User> GetAllByLoginAsync(string login)
        {
            return (await _userGR.GetDbSet()
                                 .Where(x => x.Login == login)
                                 .Include(x => x.Role)
                                 .Include(x => x.Permissions)
                                 .ThenInclude(x => x.Permission)
                                 .Include(x => x.Notifications)
                                 .ToArrayAsync()).FirstOrDefault();
        }

        public void SetPassword(User user, string password)
        {
            user = _userGR.Find(user.Id);
            if (user == null)
                throw new ClientException("u-inc-user-id");
            user.PasswordHash = PasswordHandler.CreatePasswordHash(password);
            _userGR.Update(user);
        }

        public async Task SetPasswordAsync(User user, string password)
        {
            user = await _userGR.FindAsync(user.Id);
            if (user == null)
                throw new ClientException("u-inc-user-id");
            user.PasswordHash = PasswordHandler.CreatePasswordHash(password);
            await _userGR.UpdateAsync(user);
        }

        public void ChangeLogin(string Login, User user)
        {
            user = _userGR.Find(user.Id);
            user.Login = Login;
            _userGR.Update(user);
        }

        public async Task<ProfileDto> ChangeImageAsync(string image, User user)
        {
            user = await GetAsync(user.Id);
            string remPhotoId = "";
            if (image != null)
            {
                remPhotoId = user.PhotoId;
                user.PhotoId = await _imageService.Add(image);
            }
            await _userGR.UpdateAsync(user);
            if (remPhotoId != null) await _imageService.Remove(remPhotoId);
            return await GetProfileInfoAsync(user);
        }

        public async Task<ProfileDto> GetProfileInfoAsync(User user)
        {
            ProfileDto model = new ProfileDto();
            user = await GetAsync(user.Id);
            model.Login = user.Login;
            model.Email = user.Email;
            model.Name = user.Name;
            model.Surname = user.Surname;
            model.Lastname = user.Lastname;
            model.Photo = user.PhotoId;
            return model;
        }

        public async Task ChangeLoginAsync(string Login, User user)
        {
            user = await _userGR.FindAsync(user.Id);
            user.Login = Login;
            await _userGR.UpdateAsync(user);
        }

        public void Ban(User user, string reason)
        {
            user = _userGR.Find(user.Id);
            user.Banned = reason;
            _userGR.Update(user);
        }

        public void Unban(User user)
        {
            user = _userGR.Find(user.Id);
            user.Banned = null;
            _userGR.Update(user);
        }

        public async Task BanAsync(User user, string reason)
        {
            user = await _userGR.FindAsync(user.Id);
            user.Banned = reason;
            await _userGR.UpdateAsync(user);
        }

        public async Task UnbanAsync(User user)
        {
            user = await _userGR.FindAsync(user.Id);
            user.Banned = null;
            await _userGR.UpdateAsync(user);
        }
    }
}

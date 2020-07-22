using Microsoft.EntityFrameworkCore;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.Extentions;
using SchoolBridge.Helpers.Managers;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userGR;
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;

        private readonly IGenericRepository<DefaultRolePermission> _defaultRolePermissionGR;

        public UserService(IGenericRepository<User> userGR,
                            IPermissionService permissionService,
                            IRoleService roleService,
                            IGenericRepository<DefaultRolePermission> defaultRolePermissionGR,
                            ClientErrorManager clientErrorManager)
        {
            _userGR = userGR;
            _permissionService = permissionService;
            _roleService = roleService;
            _defaultRolePermissionGR = defaultRolePermissionGR;

            if (!clientErrorManager.IsIssetErrors("User"))
                clientErrorManager.AddErrors(new ClientErrors("User", new Dictionary<string, ClientError>
                {
                    { "u-inc-user-id" , new ClientError ("Incorrect user id!")}
                }));
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
         
            user.Permissions = await _permissionService.AddPermissions(user, permissions);
            return user;
        }

        public async Task<User> AddAsync(User user, string role, IEnumerable<Permission> noPermissions = null)
        {
            user.Role = await _roleService.Get(role);
            return await AddAsync(user, noPermissions);
        }

        public Task BlockAsync(User ovner, User user, string comment, DateTime unblockDate)
        {
            throw new NotImplementedException();
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
            return await _userGR.FindAsync(Id);
        }

        public async Task<bool> IsIssetAsync(string Id)
        {
            return (await GetAsync(Id)) != null;
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
            return _userGR.Find(Id);
        }

        public bool IsIsset(string Id)
        {
            return Get(Id) != null;
        }

        public async Task RemoveAsync(User user)
        {
            await _userGR.DeleteAsync(user);
        }

        public Task Unblock(User ovner, User user, string comment, DateTime unblockDate)
        {
            throw new NotImplementedException();
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

        public async Task ChangeLoginAsync(string Login, User user)
        {
            user = await _userGR.FindAsync(user.Id);
            user.Login = Login;
            await _userGR.UpdateAsync(user);
        }
    }
}

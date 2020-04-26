using Microsoft.EntityFrameworkCore;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.Extentions;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
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
        private readonly IPanelService _panelService;
        private readonly IRoleService _roleService;

        private readonly IGenericRepository<RolePanel> _rolePanelGR;

        public UserService(IGenericRepository<User> userGR,
                            IPermissionService permissionService,
                            IPanelService panelService,
                            IRoleService roleService,
                            IGenericRepository<RolePanel> rolePanelGR,
                            ClientErrorManager clientErrorManager)
        {
            _userGR = userGR;
            _permissionService = permissionService;
            _panelService = panelService;
            _roleService = roleService;
            _rolePanelGR = rolePanelGR;

            if (!clientErrorManager.IsIssetErrors("User"))
                clientErrorManager.AddErrors(new ClientErrors("User", new Dictionary<string, ClientError>
                {
                    
                }));
        }
        public async Task<User> Add(User user, IEnumerable<Panel> noPanels = null, IEnumerable<Permission> noPermissions = null)
        {
            user = await _userGR.CreateAsync(user);
            var panels = _rolePanelGR.GetDbSet().Where(x => x.RoleId == user.RoleId).Include(x => x.Panel.Permissions).ThenInclude(x => x.Permission).Include(x => x.Panel).Select(x => x.Panel).ToArray();
            var permissions = new List<Permission>();

            panels = panels.Where(x => {
                if (noPanels.FirstOrDefault(s => s.Id == x.Id) == null) {
                    permissions.AddRange(x.Permissions.Where(s => noPermissions.FirstOrDefault(z => z.Id == s.PermissionId) == null).Select(s => s.Permission));
                    return true;
                }
                return false;
            }).ToArray();
         
            user.Panels = await _panelService.AddPanels(user, panels);
            user.Permissions = await _permissionService.AddPermissions(user, permissions);
            return user;
        }

        public async Task<User> Add(User user, string role, IEnumerable<Panel> noPanels = null, IEnumerable<Permission> noPermissions = null)
        {
            user.Role = await _roleService.Get(role);
            return await Add(user, noPanels, noPermissions);
        }

        public Task Block(User ovner, User user, string comment, DateTime unblockDate)
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

        public async Task Remove(User user)
        {
            await _userGR.DeleteAsync(user);
        }

        public Task Unblock(User ovner, User user, string comment, DateTime unblockDate)
        {
            throw new NotImplementedException();
        }

        public User GetAll(string Id)
        {
            return _userGR.GetDbSet().Where(x => x.Id == Id).Include(x => x.Role).Include(x => x.Panels).ThenInclude(x => x.Panel).Include(x => x.Permissions).Include(x => x.Notifications).ToArray().FirstOrDefault();
        }

        public User GetAllByEmail(string email)
        {
            return _userGR.GetDbSet().Where(x => x.Email == email).Include(x => x.Role).Include(x => x.Panels).ThenInclude(x => x.Panel).Include(x => x.Permissions).ThenInclude(x => x.Permission).Include(x => x.Notifications).ToArray().FirstOrDefault();
        }

        public User GetAllByLogin(string login)
        {
            return _userGR.GetDbSet().Where(x => x.Login == login).Include(x => x.Role).Include(x => x.Panels).ThenInclude(x => x.Panel).Include(x => x.Permissions).ThenInclude(x => x.Permission).Include(x => x.Notifications).ToArray().FirstOrDefault();
        }

        public async Task<User> GetAllAsync(string Id)
        {
            return (await _userGR.GetDbSet().Where(x => x.Id == Id).Include(x => x.Role).Include(x => x.Panels).ThenInclude(x => x.Panel).Include(x => x.Permissions).ThenInclude(x => x.Permission).Include(x => x.Notifications).ToArrayAsync()).FirstOrDefault();
        }

        public async Task<User> GetAllByEmailAsync(string email)
        {
            return (await _userGR.GetDbSet().Where(x => x.Email == email).Include(x => x.Role).Include(x => x.Panels).ThenInclude(x => x.Panel).Include(x => x.Permissions).ThenInclude(x => x.Permission).Include(x => x.Notifications).ToArrayAsync()).FirstOrDefault();
        }

        public async Task<User> GetAllByLoginAsync(string login)
        {
            return (await _userGR.GetDbSet().Where(x => x.Login == login).Include(x => x.Role).Include(x => x.Panels).ThenInclude(x => x.Panel).Include(x => x.Permissions).ThenInclude(x => x.Permission).Include(x => x.Notifications).ToArrayAsync()).FirstOrDefault();
        }
    }
}

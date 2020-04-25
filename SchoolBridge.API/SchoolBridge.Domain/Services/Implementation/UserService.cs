using Microsoft.EntityFrameworkCore;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.Extentions;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userGR;
        private readonly IPermissionService _permissionService;
        private readonly IPanelService _panelService;
        private readonly IRoleService _roleService;

        private readonly IGenericRepository<PanelPermission> _panelPermissionGR;
        private readonly IGenericRepository<RolePanel> _rolePanelGR;

        public UserService(IGenericRepository<User> userGR,
                            IPermissionService permissionService,
                            IPanelService panelService,
                            IRoleService roleService,
                            IGenericRepository<PanelPermission> panelPermissionGR,
                            IGenericRepository<RolePanel> rolePanelGR,
                            ClientErrorManager clientErrorManager)
        {
            _userGR = userGR;
            _permissionService = permissionService;
            _panelService = panelService;
            _roleService = roleService;
            _panelPermissionGR = panelPermissionGR;
            _rolePanelGR = rolePanelGR;

            if (!clientErrorManager.IsIssetErrors("User"))
                clientErrorManager.AddErrors(new ClientErrors("User", new Dictionary<string, ClientError>
                {
                    
                }));
        }
        public async Task<User> Add(User user, IEnumerable<Panel> noPanels = null, IEnumerable<Permission> noPermissions = null)
        {
            user.RoleId = user.Role.Id;
            user.Role = null;
            user = await _userGR.CreateAsync(user);

            var panels = _rolePanelGR.GetDbSet().Where(x => x.RoleId == user.RoleId).Include(x => x.Panel.Permissions).Select(x => x.Panel).ToArray();
            var permissions = new List<Permission>();

            panels = panels.Where(x => {
                if (noPanels.FirstOrDefault(s => s.Id == x.Id) == null) {
                    permissions.AddRange(x.Permissions.Where(s => noPermissions.FirstOrDefault(z => z.Id == s.PermissionId) == null).Select(s => new Permission { Id = s.PermissionId }));
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

        public async Task<User> Get(string Id)
        {
            return await _userGR.FindAsync(Id);
        }

        public async Task<bool> IsIsset(string Id)
        {
            return (await Get(Id)) != null;
        }

        public async Task Remove(User user)
        {
            await _userGR.DeleteAsync(user);
        }

        public Task Unblock(User ovner, User user, string comment, DateTime unblockDate)
        {
            throw new NotImplementedException();
        }
    }
}

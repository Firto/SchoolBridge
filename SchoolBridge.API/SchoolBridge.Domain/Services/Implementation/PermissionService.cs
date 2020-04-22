using AutoMapper;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class PermissionService : IPermissionService
    {
        private readonly IGenericRepository<UserPermission> _userPermissionGR;
        private readonly IGenericRepository<PanelPermission> _panelPermissionGR;
        private readonly IGenericRepository<Permission> _permissionGR;
        private readonly IMapper _mapper;
        public PermissionService(IGenericRepository<UserPermission> userPermissionGR,
                                IGenericRepository<PanelPermission> panelPermissionGR,
                                IGenericRepository<Permission> permissionGR,
                                IMapper mapper,
                                ClientErrorManager clientErrorManager)
        {
            _userPermissionGR = userPermissionGR;
            _panelPermissionGR = panelPermissionGR;
            _permissionGR = permissionGR;
            _mapper = mapper;

            if (!clientErrorManager.IsIssetErrors("Permission"))
                clientErrorManager.AddErrors(new ClientErrors("Permission", new Dictionary<string, ClientError>
                {
                    { "inc-perm-name", new ClientError("Incorrect permission name!") },
                    { "no-access", new ClientError("No access to this action!") }
                }));
        }

        public async Task<PanelPermission> AddDefaultPermission(Panel panel, Permission permission)
        {
            return await _panelPermissionGR.CreateAsync(new PanelPermission { PanelId = panel.Id, PermissionId = permission.Id });
        }

        public async Task<IEnumerable<PanelPermission>> AddDefaultPermissions(Panel panel, IEnumerable<Permission> permissions)
        {
            var template = new PanelPermission { PanelId = panel.Id };
            var arr = new PanelPermission[permissions.Count()];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (PanelPermission)template.Clone();
                arr[i].Permission = permissions.ElementAt(i);
            }
            return await _panelPermissionGR.CreateAsync(arr);
        }

        public async Task<UserPermission> AddPermission(User user, Permission permission)
        {
            return await _userPermissionGR.CreateAsync(new UserPermission { UserId = user.Id, PermissionId = permission.Id });
        }

        public async Task<IEnumerable<UserPermission>> AddPermissions(User user, IEnumerable<Permission> permissions)
        {
            var template = new UserPermission { UserId = user.Id };
            var arr = new UserPermission[permissions.Count()];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (UserPermission)template.Clone();
                arr[i].Permission = permissions.ElementAt(i);
            }
            return await _userPermissionGR.CreateAsync(arr);
        }

        public async Task<IEnumerable<Permission>> GetAllPermissions()
        {
            return await  _permissionGR.GetAllAsync();
        }

        public async Task<IEnumerable<Permission>> GetDefaultPermissions(Panel panel)
        {
            return _mapper.Map<IEnumerable<PanelPermission>, IEnumerable<Permission>>(await _panelPermissionGR.GetAllIncludeAsync((x) => x.PanelId == panel.Id, (x) => x.Permission));
        }

        public async Task<IEnumerable<Permission>> GetPermissions(User user)
        {
            return _mapper.Map<IEnumerable<UserPermission>, IEnumerable<Permission>>(await _userPermissionGR.GetAllIncludeAsync((x) => x.UserId == user.Id, (x) => x.Permission));
        }

        public async Task RemoveDefaultPermission(Panel panel, Permission permission)
        {
            await _panelPermissionGR.DeleteAsync((x) => x.PanelId == panel.Id && x.PermissionId == permission.Id);
        }

        public async Task RemoveDefaultPermissions(Panel panel, IEnumerable<Permission> permissions)
        {
            await _panelPermissionGR.DeleteAsync((x) => x.PanelId == panel.Id && permissions.FirstOrDefault((s) => s.Id == x.PermissionId) != null);
        }

        public async Task RemovePermission(User user, Permission permission)
        {
            await _userPermissionGR.DeleteAsync((x) => x.UserId == user.Id && x.PermissionId == permission.Id);
        }

        public async Task RemovePermissions(User user, IEnumerable<Permission> permissions)
        {
            await _userPermissionGR.DeleteAsync((x) => x.UserId == user.Id && permissions.FirstOrDefault((s) => s.Id == x.PermissionId) != null);
        }

        public bool HasPermission(User user, Permission permission)
        {
            return _userPermissionGR.CountWhere((x) => x.UserId == user.Id && x.PermissionId == permission.Id) > 0;
        }

        public bool HasAllPermissions(User user, IEnumerable<Permission> permissions)
        {
            return _userPermissionGR.CountWhere((x) => x.UserId == user.Id && permissions.FirstOrDefault((s) => s.Id == x.PermissionId) != null) == permissions.Count();
        }

        public bool HasPermission(User user, string pName)
        {
            var p = _permissionGR.GetAll((x) => x.Name == pName).FirstOrDefault();
            if (p == null)
                throw new ClientException("inc-perm-name");
            return HasPermission(user, p);
        }

        public bool HasAllPermissions(User user, IEnumerable<string> pNames)
        {
            var p = _permissionGR.GetAll((x) => pNames.FirstOrDefault((s) => s == x.Name) != null);

            if (p.Count() == 0)
                throw new ClientException("inc-perm-name");
            return HasAllPermissions(user, p);
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IGenericRepository<DefaultRolePermission> _defaultRolePermissionGR;
        private readonly IGenericRepository<Permission> _permissionGR;
        public PermissionService(IGenericRepository<UserPermission> userPermissionGR,
                                IGenericRepository<DefaultRolePermission> defaultRolePermissionGR,
                                IGenericRepository<Permission> permissionGR,
                                ClientErrorManager clientErrorManager)
        {
            _userPermissionGR = userPermissionGR;
            _defaultRolePermissionGR = defaultRolePermissionGR;
            _permissionGR = permissionGR;

            if (!clientErrorManager.IsIssetErrors("Permission"))
                clientErrorManager.AddErrors(new ClientErrors("Permission", new Dictionary<string, ClientError>
                {
                    { "inc-perm-name", new ClientError("Incorrect permission name!") },
                    { "no-access", new ClientError("No access to this action!") }
                }));
        }

        public async Task<IEnumerable<Permission>> GetAllPermissions()
        {
            return await _permissionGR.GetAllAsync();
        }

        public async Task<IEnumerable<Permission>> GetDefaultPermissions(Role role)
        {
            return (await _defaultRolePermissionGR.GetAllIncludeAsync((x) => x.RoleId == role.Id, (x) => x.Permission)).Select(x => new Permission { Id = x.PermissionId });
        }

        public async Task<IEnumerable<Permission>> GetPermissions(User user)
        {
            return (await _userPermissionGR.GetAllIncludeAsync((x) => x.UserId == user.Id, (x) => x.Permission)).Select(x => new Permission { Id = x.PermissionId });
        }

        public async Task<DefaultRolePermission> AddDefaultPermission(Role role, Permission permission)
        {
            return await _defaultRolePermissionGR.CreateAsync(new DefaultRolePermission { RoleId = role.Id, PermissionId = permission.Id });
        }

        public async Task<IEnumerable<DefaultRolePermission>> AddDefaultPermissions(Role role, IEnumerable<Permission> permissions)
        {
            return await _defaultRolePermissionGR.CreateAsync(permissions.Select(x => new DefaultRolePermission { RoleId = role.Id, PermissionId = x.Id }));
        }

        public async Task<UserPermission> AddPermission(User user, Permission permission)
        {
            return await _userPermissionGR.CreateAsync(new UserPermission { UserId = user.Id, PermissionId = permission.Id });
        }

        public async Task<IEnumerable<UserPermission>> AddPermissions(User user, IEnumerable<Permission> permissions)
        {
            return await _userPermissionGR.CreateAsync(permissions.Select(x => new UserPermission { UserId = user.Id, PermissionId = x.Id }));
        }


        public async Task RemoveDefaultPermission(Role role, Permission permission)
        {
            await _defaultRolePermissionGR.DeleteAsync((x) => x.RoleId == role.Id && x.PermissionId == permission.Id);
        }

        public async Task RemoveDefaultPermissions(Role role, IEnumerable<Permission> permissions)
        {
            await _defaultRolePermissionGR.DeleteAsync((x) => x.RoleId == role.Id && permissions.FirstOrDefault((s) => s.Id == x.PermissionId) != null);
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
            return _userPermissionGR.GetDbSet()
                                    .Where((x) => x.UserId == user.Id)
                                    .ToArray()
                                    .TakeWhile(x => permissions.FirstOrDefault(s => s.Id == x.PermissionId) != null)
                                    .Count() != permissions.Count();
        }

        public bool HasPermission(User user, string pName)
        {
            return _userPermissionGR.GetDbSet().Where((x) => x.UserId == user.Id && x.Permission.Name == pName).Include(x => x.Permission).Count() > 0;
        }

        public bool HasAllPermissions(User user, IEnumerable<string> pNames)
        {
            return _userPermissionGR.GetDbSet()
                                    .Where((x) => x.UserId == user.Id)
                                    .Include(x => x.Permission)
                                    .ToArray()
                                    .TakeWhile(x => pNames.Contains(x.Permission.Name))
                                    .Count() != pNames.Count();
        }

        
    }
}

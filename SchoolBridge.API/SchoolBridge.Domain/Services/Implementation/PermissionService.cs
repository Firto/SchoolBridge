using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Managers.CClientErrorManager;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class PermissionService : IPermissionService
    {
        private readonly IGenericRepository<UserPermission> _userPermissionGR;
        private readonly IGenericRepository<DefaultRolePermission> _defaultRolePermissionGR;
        private readonly IGenericRepository<Permission> _permissionGR;
        public PermissionService(IGenericRepository<UserPermission> userPermissionGR,
                                IGenericRepository<DefaultRolePermission> defaultRolePermissionGR,
                                IGenericRepository<Permission> permissionGR)
        {
            _userPermissionGR = userPermissionGR;
            _defaultRolePermissionGR = defaultRolePermissionGR;
            _permissionGR = permissionGR; 
        }

        public static void OnInit(ClientErrorManager manager)
        {
            manager.AddErrors(new ClientErrors("Permission", new Dictionary<string, ClientError>
                {
                    { "inc-perm-name", new ClientError("Incorrect permission name!") },
                    { "no-access", new ClientError("No access to this action!") }
                }));
        }

        public static void OnFirstInit(IPermissionService permissionService, IGenericRepository<Role> roleGR)
        {
            var pupilPerms = new Permission[] {
                new Permission { Name = "GetPupilPanel" },
                    new Permission { Name = "EditSubject" },
            };

            var adminPerms = new Permission[] {
                new Permission { Name = "GetAdminPanel"},
                    new Permission { Name = "GetGlobalizationTab" },

                        new Permission { Name = "GetLanguagesEditTab" },
                            new Permission { Name = "CreateLanguage" },
                            new Permission { Name = "RemoveLanguage" },
                            new Permission { Name = "EditLanguage" },

                        new Permission { Name = "GetStringsEditTab" },
                            new Permission { Name = "UpdateBaseUpdateId" },
                            new Permission { Name = "EditGbStrings" },

                    new Permission { Name = "GetAllUsers"},
                        new Permission { Name = "EditUser"},         
                        new Permission { Name = "BanUser"},
                        new Permission { Name = "UnbanUser"}
            };

            permissionService.AddPermissions(adminPerms.Concat(pupilPerms));

            var admRole = roleGR.GetAll(x => x.Name == "Admin").FirstOrDefault();
            admRole.DefaultPermissions = adminPerms.Select(x => new DefaultRolePermission { Permission = x }).ToList();
            roleGR.Update(admRole);

            var pupilRole = roleGR.GetAll(x => x.Name == "Pupil").FirstOrDefault();
            pupilRole.DefaultPermissions = pupilPerms.Select(x => new DefaultRolePermission { Permission = x }).ToList();
            roleGR.Update(pupilRole);

            permissionService.AddUserPermissions(new User { Id = "admin" }, adminPerms.Concat(pupilPerms));
        }

        public Permission AddPermission(Permission permission) 
            => _permissionGR.Create(permission);
        public void RemovePermission(Permission permission)
            => _permissionGR.Delete(permission);

        public IEnumerable<Permission> AddPermissions(IEnumerable<Permission> permissions)
            => _permissionGR.Create(permissions);
        public void RemovePermissions(IEnumerable<Permission> permissions)
            => _permissionGR.Delete(permissions);

        public UserPermission AddUserPermission(User user, Permission permission)
        {
            return _userPermissionGR.Create(new UserPermission { UserId = user.Id, PermissionId = permission.Id });
        }

        public IEnumerable<UserPermission> AddUserPermissions(User user, IEnumerable<Permission> permissions)
        {
            return _userPermissionGR.Create(permissions.Select(x => new UserPermission { UserId = user.Id, PermissionId = x.Id }));
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await _permissionGR.GetAllAsync();
        }

        public async Task<IEnumerable<Permission>> GetDefaultPermissionsAsync(Role role)
        {
            return (await _defaultRolePermissionGR.GetAllIncludeAsync((x) => x.RoleId == role.Id, (x) => x.Permission)).Select(x => new Permission { Id = x.PermissionId });
        }

        public async Task<IEnumerable<Permission>> GetPermissionsAsync(User user)
        {
            return (await _userPermissionGR.GetAllIncludeAsync((x) => x.UserId == user.Id, (x) => x.Permission)).Select(x => new Permission { Id = x.PermissionId });
        }

        public async Task<DefaultRolePermission> AddDefaultPermissionAsync(Role role, Permission permission)
        {
            return await _defaultRolePermissionGR.CreateAsync(new DefaultRolePermission { RoleId = role.Id, PermissionId = permission.Id });
        }

        public async Task<IEnumerable<DefaultRolePermission>> AddDefaultPermissionsAsync(Role role, IEnumerable<Permission> permissions)
        {
            return await _defaultRolePermissionGR.CreateAsync(permissions.Select(x => new DefaultRolePermission { RoleId = role.Id, PermissionId = x.Id }));
        }

        public async Task<UserPermission> AddUserPermissionAsync(User user, Permission permission)
        {
            return await _userPermissionGR.CreateAsync(new UserPermission { UserId = user.Id, PermissionId = permission.Id });
        }

        public async Task<IEnumerable<UserPermission>> AddUserPermissionsAsync(User user, IEnumerable<Permission> permissions)
        {
            return await _userPermissionGR.CreateAsync(permissions.Select(x => new UserPermission { UserId = user.Id, PermissionId = x.Id }));
        }

        public async Task RemoveDefaultPermissionAsync(Role role, Permission permission)
        {
            await _defaultRolePermissionGR.DeleteAsync((x) => x.RoleId == role.Id && x.PermissionId == permission.Id);
        }

        public async Task RemoveDefaultPermissionsAsync(Role role, IEnumerable<Permission> permissions)
        {
            await _defaultRolePermissionGR.DeleteAsync((x) => x.RoleId == role.Id && permissions.FirstOrDefault((s) => s.Id == x.PermissionId) != null);
        }

        public async Task RemovePermissionAsync(User user, Permission permission)
        {
            await _userPermissionGR.DeleteAsync((x) => x.UserId == user.Id && x.PermissionId == permission.Id);
        }

        public async Task RemovePermissionsAsync(User user, IEnumerable<Permission> permissions)
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

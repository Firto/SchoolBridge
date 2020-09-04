using SchoolBridge.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IPermissionService: IOnInitService, IOnFirstInitService
    {
        Permission AddPermission(Permission permission);
        IEnumerable<Permission> AddPermissions(IEnumerable<Permission> permissions);
        void RemovePermission(Permission permission);
        void RemovePermissions(IEnumerable<Permission> permissions);

        UserPermission AddUserPermission(User user, Permission permission);
        IEnumerable<UserPermission> AddUserPermissions(User user, IEnumerable<Permission> permissions);

        Task<IEnumerable<Permission>> GetAllPermissionsAsync();
        Task<IEnumerable<Permission>> GetPermissionsAsync(User user);
        Task<IEnumerable<Permission>> GetDefaultPermissionsAsync(Role role);

        Task<UserPermission> AddUserPermissionAsync(User user, Permission permission);
        Task<IEnumerable<UserPermission>> AddUserPermissionsAsync(User user, IEnumerable<Permission> permissions);
        Task<DefaultRolePermission> AddDefaultPermissionAsync(Role role, Permission permission);
        Task<IEnumerable<DefaultRolePermission>> AddDefaultPermissionsAsync(Role role, IEnumerable<Permission> permissions);

        Task RemovePermissionAsync(User user, Permission permission);
        Task RemovePermissionsAsync(User user, IEnumerable<Permission> permissions);
        Task RemoveDefaultPermissionAsync(Role panel, Permission permission);
        Task RemoveDefaultPermissionsAsync(Role panel, IEnumerable<Permission> permissions);

        bool HasPermission(User user, Permission permission);
        bool HasAllPermissions(User user, IEnumerable<Permission> permissions);
        bool HasPermission(User user, string pName);
        bool HasAllPermissions(User user, IEnumerable<string> pNames);
    }
}

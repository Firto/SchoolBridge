using SchoolBridge.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IPermissionService
    {
        Task<IEnumerable<Permission>> GetAllPermissions();
        Task<IEnumerable<Permission>> GetPermissions(User user);
        Task<IEnumerable<Permission>> GetDefaultPermissions(Role role);

        Task<UserPermission> AddPermission(User user, Permission permission);
        Task<IEnumerable<UserPermission>> AddPermissions(User user, IEnumerable<Permission> permissions);
        Task<DefaultRolePermission> AddDefaultPermission(Role role, Permission permission);
        Task<IEnumerable<DefaultRolePermission>> AddDefaultPermissions(Role role, IEnumerable<Permission> permissions);

        Task RemovePermission(User user, Permission permission);
        Task RemovePermissions(User user, IEnumerable<Permission> permissions);
        Task RemoveDefaultPermission(Role panel, Permission permission);
        Task RemoveDefaultPermissions(Role panel, IEnumerable<Permission> permissions);

        bool HasPermission(User user, Permission permission);
        bool HasAllPermissions(User user, IEnumerable<Permission> permissions);
        bool HasPermission(User user, string pName);
        bool HasAllPermissions(User user, IEnumerable<string> pNames);
    }
}

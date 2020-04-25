using SchoolBridge.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IPermissionService
    {
        Task<IEnumerable<Permission>> GetAllPermissions();
        Task<IEnumerable<Permission>> GetPermissions(User user);
        Task<IEnumerable<Permission>> GetDefaultPermissions(Panel panel);
        Task<IEnumerable<Permission>> GetDefaultPermissions(IEnumerable<Panel> panels);

        Task<UserPermission> AddPermission(User user, Permission permission);
        Task<IEnumerable<UserPermission>> AddPermissions(User user, IEnumerable<Permission> permissions);
        Task<PanelPermission> AddDefaultPermission(Panel panel, Permission permission);
        Task<IEnumerable<PanelPermission>> AddDefaultPermissions(Panel panel, IEnumerable<Permission> permissions);

        Task RemovePermission(User user, Permission permission);
        Task RemovePermissions(User user, IEnumerable<Permission> permissions);
        Task RemoveDefaultPermission(Panel panel, Permission permission);
        Task RemoveDefaultPermissions(Panel panel, IEnumerable<Permission> permissions);

        bool HasPermission(User user, Permission permission);
        bool HasAllPermissions(User user, IEnumerable<Permission> permissions);
        bool HasPermission(User user, string pName);
        bool HasAllPermissions(User user, IEnumerable<string> pNames);
    }
}

using SchoolBridge.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IUserService
    {
        Task<User> Add(User user, IEnumerable<Panel> noPanels = null, IEnumerable<Permission> noPermissions = null);
        Task<User> Add(User user, string role, IEnumerable<Panel> noPanels = null, IEnumerable<Permission> noPermissions = null);
        Task<User> Get(string Id);
        Task<bool> IsIsset(string Id);
        Task Remove(User user);
        Task Block(User ovner, User user, string comment, DateTime unblockDate);
        Task Unblock(User ovner, User user, string comment, DateTime unblockDate);
    }
}

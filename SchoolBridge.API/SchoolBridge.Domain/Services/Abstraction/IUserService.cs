using SchoolBridge.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IUserService
    {
        User Get(string Id);
        User GetByEmail(string email);
        User GetByLogin(string login);

        User GetAll(string Id);
        User GetAllByEmail(string email);
        User GetAllByLogin(string login);

        bool IsIsset(string Id);
        bool IsIssetByEmail(string email);
        bool IsIssetByLogin(string login);

        Task<User> GetAsync(string Id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByLoginAsync(string login);

        Task<User> GetAllAsync(string Id);
        Task<User> GetAllByEmailAsync(string email);
        Task<User> GetAllByLoginAsync(string login);

        Task<bool> IsIssetAsync(string Id);
        Task<bool> IsIssetByEmailAsync(string email);
        Task<bool> IsIssetByLoginAsync(string login);

        Task<User> Add(User user, IEnumerable<Panel> noPanels = null, IEnumerable<Permission> noPermissions = null);
        Task<User> Add(User user, string role, IEnumerable<Panel> noPanels = null, IEnumerable<Permission> noPermissions = null);
        
        Task Remove(User user);
        Task Block(User ovner, User user, string comment, DateTime unblockDate);
    }
}

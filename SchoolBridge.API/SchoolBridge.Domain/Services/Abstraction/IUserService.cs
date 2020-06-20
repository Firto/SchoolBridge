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

        void SetPassword(User user, string password);

        Task<User> GetAsync(string Id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByLoginAsync(string login);

        Task<User> GetAllAsync(string Id);
        Task<User> GetAllByEmailAsync(string email);
        Task<User> GetAllByLoginAsync(string login);

        Task<bool> IsIssetAsync(string Id);
        Task<bool> IsIssetByEmailAsync(string email);
        Task<bool> IsIssetByLoginAsync(string login);

        Task<User> AddAsync(User user, IEnumerable<Permission> noPermissions = null);
        Task<User> AddAsync(User user, string role, IEnumerable<Permission> noPermissions = null);

        Task SetPasswordAsync(User user, string password);

        Task RemoveAsync(User user);
        Task BlockAsync(User ovner, User user, string comment, DateTime unblockDate);
    }
}

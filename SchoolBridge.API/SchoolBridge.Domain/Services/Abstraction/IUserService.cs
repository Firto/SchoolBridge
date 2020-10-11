using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Helpers.DtoModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IUserService: IOnInitService, IOnFirstInitService
    {
        UserDto GetFullDto(string tokenId, string Id);
        UserDto GetFullDtoByGetToken(string tokenId, string getToken);
        ShortUserDto GetShortDto(string Id);
        ShortUserDto GetStaticShortDto(string Id);

        Task<UserDto> GetFullDtoAsync(string tokenId, string Id);
        Task<UserDto> GetFullDtoByGetTokenAsync(string tokenId, string getToken);

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
        void ChangeLogin(string Login, User user);

        Task<User> GetAsync(string Id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByLoginAsync(string login);
        Task<ProfileDto> GetProfileInfoAsync(User user);

        Task<User> GetAllAsync(string Id);
        Task<User> GetAllByEmailAsync(string email);
        Task<User> GetAllByLoginAsync(string login);

        Task<bool> IsIssetAsync(string Id);
        Task<bool> IsIssetByEmailAsync(string email);
        Task<bool> IsIssetByLoginAsync(string login);

        Task<User> AddAsync(User user, IEnumerable<Permission> noPermissions = null);
        Task<User> AddAsync(User user, string role, IEnumerable<Permission> noPermissions = null);

        Task SetPasswordAsync(User user, string password);
        Task ChangeLoginAsync(string Login, User user);
        Task<ProfileDto> ChangeImageAsync(string Image, User user);


        void Ban(User user, string reason);
        void Unban(User user);

        Task BanAsync(User user, string reason);
        Task UnbanAsync(User user);
    }
}

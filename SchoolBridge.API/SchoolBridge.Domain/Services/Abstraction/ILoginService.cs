using System.Threading.Tasks;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Helpers.DtoModels;
using Microsoft.AspNetCore.Http;
using SchoolBridge.Helpers.DtoModels.Authefication;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface ILoginService
    {
        //security
        Task<LoggedTokensDto> RefreshToken(RefreshTokenDto entity, string uuid);
        Task<LoggedDto> Login(LoginDto entity, string uuid);
        Task<LoggedDto> Login(User user, string uuid);
        Task Logout(IHeaderDictionary headers);
        Task LogoutAll(IHeaderDictionary headers);
        // basic
        //Task<ProfileDto> SetProfileInfo(User user, ProfileDto entity);
        //Task<ProfileDto> GetProfileInfo(User user);
    }
}
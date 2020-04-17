using SchoolBridge.DataAccess.Entities.Authorization;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using SchoolBridge.Helpers.DtoModels.Authefication;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface ITokenService<AUser> where AUser : AuthUser
    {
        int CountLoggedDevices(AUser user);

        Task<LoggedDto> Login(AUser user, string uuid);
        Task<LoggedDto> RefreshToken(string token, string uuid);

        AUser GetUser(string token);
        AUser GetUser(IHeaderDictionary headers);
        AUser GetUser(HttpContext context);

        JwtSecurityToken ValidateRefreshToken(string token);
        JwtSecurityToken ValidateToken(string token);
        JwtSecurityToken ValidateToken(IHeaderDictionary headers);
        JwtSecurityToken ValidateToken(HttpContext context);

        Task DeactivateToken(string token);
        Task DeactivateToken(IHeaderDictionary headers);
        Task DeactivateToken(HttpContext context);

        Task DeactivateAllTokens(string token);
        Task DeactivateAllTokens(IHeaderDictionary headers);
        Task DeactivateAllTokens(HttpContext context);
    }
}
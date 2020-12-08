using SchoolBridge.DataAccess.Entities.Authorization;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using SchoolBridge.Helpers.DtoModels.Authefication;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface ITokenService: IOnInitService
    {
        int CountLoggedDevices(string userId);

        Task<LoggedTokensDto> Login(string userId, string uuid);
        Task<LoggedTokensDto> RefreshToken(string token, string uuid);

        string GetUser(string token);
        string GetUser(IHeaderDictionary headers);
        string GetUser(HttpContext context);

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
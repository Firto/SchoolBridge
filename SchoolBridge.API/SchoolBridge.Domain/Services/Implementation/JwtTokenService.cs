using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SchoolBridge.Helpers.DtoModels.Authefication;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class JwtTokenService<AUser> : ITokenService<AUser> where AUser : AuthUser
    {
        private readonly IGenericRepository<ActiveRefreshToken<AUser>> _activeRefreshTokensGR;
        private readonly IGenericRepository<AUser> _usersGR;
        private readonly TokenServiceConfiguration _configuration;

        private string TakeBearerTokenFromHeader(string header)
            => new string(header.Skip(7).ToArray());

        public JwtTokenService(IGenericRepository<ActiveRefreshToken<AUser>> activeRefreshTokensGR,
                               IGenericRepository<AUser> usersGR,
                               TokenServiceConfiguration configuration,
                               ClientErrorManager clientErrorManager) {
            _activeRefreshTokensGR = activeRefreshTokensGR;
            _usersGR = usersGR;
            _configuration = configuration;

            if (!clientErrorManager.IsIssetErrors("Token"))
                clientErrorManager.AddErrors(new ClientErrors("Token", new Dictionary<string, ClientError> {
                    {"inc-refresh-token", new ClientError("Incorrect refresh token!")},
                    {"inc-token", new ClientError("Incorrect token!")},
                    {"no-uuid", new ClientError("No UUID!")},
                    {"inc-uuid", new ClientError("Incorrect UUID!")},
                    {"no-token", new ClientError("No token!")}
                }));
        }

        public int CountLoggedDevices(AUser user)
            => _activeRefreshTokensGR.CountWhere((x) => x.UserId == user.Id);

        // Validating

        public JwtSecurityToken ValidateToken(string token)
        {
            SecurityToken validatedToken;
            try
            {
                _configuration.TokenHandler.ValidateToken(token, _configuration.TokenValidation, out validatedToken);
            }
            catch (Exception)
            {
                throw new ClientException("inc-token");
            }

            return validatedToken as JwtSecurityToken;
        }

        public JwtSecurityToken ValidateToken(IHeaderDictionary headers)
        {
            if (!headers.ContainsKey("Authorization"))
                throw new ClientException("inc-token");

            return ValidateToken(TakeBearerTokenFromHeader(headers["Authorization"]));
        }

        public JwtSecurityToken ValidateToken(HttpContext context)
            => ValidateToken(context.Request.Headers);

        public JwtSecurityToken ValidateRefreshToken(string token)
        {
            SecurityToken validatedToken;
            try
            {
                _configuration.TokenHandler.ValidateToken(token, _configuration.RefreshTokenValidation, out validatedToken);
            }
            catch (Exception)
            {
                throw new ClientException("inc-refresh-token");
            }

            return validatedToken as JwtSecurityToken;
        }

        // Deactivating

        public async Task DeactivateToken(string token)
        {
            JwtSecurityToken stoken = ValidateToken(token);
            ActiveRefreshToken<AUser> tkn = await _activeRefreshTokensGR.FindAsync(stoken.Id);

            if (tkn == null)
                throw new ClientException("inc-token");

            _activeRefreshTokensGR.DeleteAsync(tkn);
        }

        public async Task DeactivateToken(IHeaderDictionary headers)
        {
            if (!headers.ContainsKey("Authorization"))
                throw new ClientException("inc-token");

            await DeactivateToken(TakeBearerTokenFromHeader(headers["Authorization"]));
        }

        public async Task DeactivateToken(HttpContext context)
            => await DeactivateToken(context.Request.Headers);

        public async Task DeactivateAllTokens(string token)
        {
            JwtSecurityToken stoken = ValidateToken(token);
            ActiveRefreshToken<AUser> tkn = (await _activeRefreshTokensGR.GetAllIncludeAsync((x) => x.Jti == stoken.Id, (x) => x.User)).FirstOrDefault();

            if (tkn == null)
                throw new ClientException("inc-token");

            _activeRefreshTokensGR.DeleteAsync((x) => x.UserId == tkn.UserId);
        }

        public async Task DeactivateAllTokens(IHeaderDictionary headers)
        {
            if (!headers.ContainsKey("Authorization"))
                throw new ClientException("inc-token");

            await DeactivateAllTokens(TakeBearerTokenFromHeader(headers["Authorization"]));
        }

        public async Task DeactivateAllTokens(HttpContext context)
            => await DeactivateAllTokens(context.Request.Headers);

        // base methods

        public async Task<LoggedDto> RefreshToken(string token, string uuid)
        {
            JwtSecurityToken to = ValidateRefreshToken(token);
            ActiveRefreshToken<AUser> activeJwtRefreshToken = _activeRefreshTokensGR.GetAllInclude((x) => x.Jti == to.Id, (s) => s.User).FirstOrDefault();

            if (activeJwtRefreshToken == null)
                throw new ClientException("inc-refresh-token");

            if (activeJwtRefreshToken.UUID != uuid)
                throw new ClientException("inc-uuid");

            await _activeRefreshTokensGR.DeleteAsync(activeJwtRefreshToken);
            return await Login(activeJwtRefreshToken.User, activeJwtRefreshToken.UUID);
        }

        public async Task<LoggedDto> Login(AUser user, string uuid) {
            var refreshExpires = DateTime.Now.Add(_configuration.RefreshTokenExpires);
            var expires = DateTime.Now.Add(_configuration.TokenExpires);

            var jti = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, jti)
            };

            var creds = new SigningCredentials(_configuration.TokenValidation.IssuerSigningKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.TokenValidation.ValidIssuer,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            claims.RemoveAll((x) => x.Type == JwtRegisteredClaimNames.Sub);
            creds = new SigningCredentials(_configuration.RefreshTokenValidation.IssuerSigningKey, SecurityAlgorithms.HmacSha256);

            var refreshToken = new JwtSecurityToken(
               issuer: _configuration.RefreshTokenValidation.ValidIssuer,
               claims: claims,
               expires: refreshExpires,
               signingCredentials: creds
            );

            await _activeRefreshTokensGR.DeleteAsync((x) => x.UUID == uuid && x.UserId == user.Id);
            await _activeRefreshTokensGR.CreateAsync(new ActiveRefreshToken<AUser> { Jti = jti, UUID = uuid, User = user, Expire = refreshExpires });
            return new LoggedDto { RefreshToken = _configuration.TokenHandler.WriteToken(refreshToken), Token = _configuration.TokenHandler.WriteToken(token), Expires = expires.ToUnixTimestamp() };
        }

        public AUser GetUser(string token)
        {
            JwtSecurityToken stoken = ValidateToken(token);
            AUser usr = _usersGR.Find(stoken.Subject);
            if (usr == null)
                throw new ClientException("inc-token");
            return usr;
        }

        public AUser GetUser(IHeaderDictionary headers)
        {
            if (!headers.ContainsKey("Authorization"))
                throw new ClientException("inc-token");

            return GetUser(TakeBearerTokenFromHeader(headers["Authorization"]));
        }

        public AUser GetUser(HttpContext context)
            => GetUser(context.Request.Headers);

    }
}
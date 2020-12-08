using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Managers.CClientErrorManager;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SchoolBridge.Helpers.DtoModels.Authefication;
using SchoolBridge.DataAccess.Entities;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class JwtTokenService : ITokenService
    {
        private readonly IGenericRepository<ActiveRefreshToken> _activeRefreshTokensGR;
        private readonly IGenericRepository<User> _usersGR;
        private readonly IUserConnectionService _userConnectionService;
        private readonly TokenServiceConfiguration _configuration;

        private string TakeBearerTokenFromHeader(string header)
            => new string(header.Skip(7).ToArray());

        public JwtTokenService(IGenericRepository<ActiveRefreshToken> activeRefreshTokensGR,
                               IGenericRepository<User> usersGR,
                               IUserConnectionService userConnectionService,
                               TokenServiceConfiguration configuration) {
            _activeRefreshTokensGR = activeRefreshTokensGR;
            _usersGR = usersGR;
            _userConnectionService = userConnectionService;
            _configuration = configuration;
        }

        public static void OnInit(ClientErrorManager manager)
        {
            manager.AddErrors(new ClientErrors("TokenService", new Dictionary<string, ClientError> {
                    {"inc-refresh-token", new ClientError("Incorrect refresh token!")},
                    {"inc-token", new ClientError("Incorrect token!")},
                    {"no-uuid", new ClientError("No UUID!")},
                    {"inc-uuid", new ClientError("Incorrect UUID!")},
                    {"no-token", new ClientError("No token!")}
                }));
        }

        public int CountLoggedDevices(string userId)
            => _activeRefreshTokensGR.CountWhere((x) => x.UserId == userId);

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
            ActiveRefreshToken tkn = await _activeRefreshTokensGR.FindAsync(stoken.Id);

            if (tkn == null)
                throw new ClientException("inc-token");

            await _activeRefreshTokensGR.DeleteAsync(tkn);
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
            ActiveRefreshToken tkn = (await _activeRefreshTokensGR.GetAllIncludeAsync((x) => x.Jti == stoken.Id, (x) => x.User)).FirstOrDefault();

            if (tkn == null)
                throw new ClientException("inc-token");

            await _activeRefreshTokensGR.DeleteAsync((x) => x.UserId == tkn.UserId);
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

        public async Task<LoggedTokensDto> RefreshToken(string token, string uuid)
        {
            JwtSecurityToken to = ValidateRefreshToken(token);
            ActiveRefreshToken activeJwtRefreshToken = _activeRefreshTokensGR.GetAll((x) => x.Jti == to.Id).FirstOrDefault();

            if (activeJwtRefreshToken == null)
                throw new ClientException("inc-refresh-token");

            if (activeJwtRefreshToken.UUID != uuid)
                throw new ClientException("inc-uuid");

            await _activeRefreshTokensGR.DeleteAsync(activeJwtRefreshToken);
            var ttoken = await Login(activeJwtRefreshToken.UserId, activeJwtRefreshToken.UUID);
            _userConnectionService.UpdateUserToken(to.Id, ttoken.Token);
            return ttoken;
        }

        public async Task<LoggedTokensDto> Login(string userId, string uuid) {
            var refreshExpires = DateTime.Now.Add(_configuration.RefreshTokenExpires);
            var expires = DateTime.Now.Add(_configuration.TokenExpires);

            var jti = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
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

            await _activeRefreshTokensGR.DeleteAsync((x) => x.UUID == uuid && x.UserId == userId);
            await _activeRefreshTokensGR.CreateAsync(new ActiveRefreshToken { Jti = jti, UUID = uuid, UserId = userId, Expire = refreshExpires });
            return new LoggedTokensDto { RefreshToken = _configuration.TokenHandler.WriteToken(refreshToken), Token = _configuration.TokenHandler.WriteToken(token), Expires = expires.ToUnixTimestamp() };
        }

        public string GetUser(string token)
        {
            JwtSecurityToken stoken = ValidateToken(token);
            User usr = _usersGR.Find(stoken.Subject);
            if (usr == null)
                throw new ClientException("inc-token");
            return usr.Id;
        }

        public string GetUser(IHeaderDictionary headers)
        {
            if (!headers.ContainsKey("Authorization"))
                throw new ClientException("inc-token");

            return GetUser(TakeBearerTokenFromHeader(headers["Authorization"]));
        }

        public string GetUser(HttpContext context)
            => GetUser(context.Request.Headers);

    }
}
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.IdentityModel.Tokens;
using SchoolBridge.Domain.Managers.CClientErrorManager;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.DtoModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class PermanentConnectionService: IPermanentConnectionService
    {
        private readonly ConcurrentDictionary<string, JwtSecurityToken> _users = new ConcurrentDictionary<string, JwtSecurityToken>();
        private readonly PermanentConnectionServiceConfiguration _configuration;

        public IDictionary<string, JwtSecurityToken> ConnectedUsers { get => _users; }

        public PermanentConnectionService(PermanentConnectionServiceConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public static void OnInit(ClientErrorManager manager)
        {
            manager.AddErrors(new ClientErrors("PermanentConnectionService",
                    new Dictionary<string, ClientError>
                    {
                        {"inc-permanent-token", new ClientError("Incorrect permanent token!")}
                    }
                ));
        }

        public string[] GetConnections(string tokenId)
        {
            return _users.Where((x) => x.Value.Id == tokenId && x.Value.Payload.Exp > DateTime.Now.ToUnixTimestamp()).Select((x) => x.Key).ToArray();
        }

        public string[] GetConnections(string[] tokenIds)
        {
            return _users.Where((x) => tokenIds.FirstOrDefault((s) => s == x.Value.Id) != null && x.Value.Payload.Exp > DateTime.Now.ToUnixTimestamp()).Select((x) => x.Key).ToArray();
        }

        public PermanentSubscribeDto CreatePermanentToken(TimeSpan? exp, out string guid)
        {
            DateTime expires = DateTime.Now;
            if (!exp.HasValue)
                expires = expires.Add(_configuration.PermanentTokenExpires);
            else expires = expires.Add(exp.Value);
            guid = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, guid)
            };

            var creds = new SigningCredentials(_configuration.PermanentTokenValidation.IssuerSigningKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.PermanentTokenValidation.ValidIssuer,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new PermanentSubscribeDto { Token = _configuration.TokenHandler.WriteToken(token), Expires = expires.ToUnixTimestamp() };
        }

        public JwtSecurityToken ValidatePermanentToken(string token)
        {
            SecurityToken validatedToken = null;
            try
            {
                _configuration.TokenHandler.ValidateToken(token, _configuration.PermanentTokenValidation, out validatedToken);
            }
            catch (Exception)
            {
                throw new ClientException("inc-permanent-token");
            }

            return validatedToken as JwtSecurityToken;
        }

        public void OnConnected(Microsoft.AspNetCore.SignalR.HubCallerContext hubCallerContext, string token)
        {
            JwtSecurityToken tkn = null;
            try
            {
                tkn = ValidatePermanentToken(token);
            }
            catch (ClientException)
            {
                return;
            }
            catch (Exception)
            {
                return;
            }
            _users.AddOrUpdate(hubCallerContext.ConnectionId, tkn, (x, s) => tkn);
        }

        public void OnDisconnected(Microsoft.AspNetCore.SignalR.HubCallerContext hubCallerContext)
        {
            JwtSecurityToken sor;
            _users.TryRemove(hubCallerContext.ConnectionId, out sor);
        }
    }
}

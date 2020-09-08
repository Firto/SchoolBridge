using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.AddtionalClases.UserConnectionService;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class UserConnectionService : IUserConnectionService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<string, UserSession> _users = new ConcurrentDictionary<string, UserSession>();

        public event UserConnectionEvent OnConnected;
        public event UserConnectionEvent OnDisconnected;

        public IDictionary<string, UserSession> ConnectedUsers { get => _users; }

        public UserConnectionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void UpdateUserToken(string oldTokenId, string newToken) {
            UserSession session = _users.FirstOrDefault(x => x.Value.TokenId == oldTokenId).Value;
            if (session == null) return;

            using (var scope = _serviceProvider.CreateScope())
            {
                var tokenService = scope.ServiceProvider.GetService<ITokenService>();
                JwtSecurityToken tkn = null;
                try
                {
                    tkn = tokenService.ValidateToken(newToken);
                }
                catch (ClientException)
                {
                    return;
                }
                catch (Exception)
                {
                    return;
                }
                session.UpdateToken(tkn);
            }
        }

        public UserSession GetUserSession(Microsoft.AspNetCore.SignalR.HubCallerContext hubCallerContext) {
            UserSession session = null;
            _users.TryGetValue(hubCallerContext.ConnectionId, out session);
            return session;
        }

        public bool GetUserSession(Microsoft.AspNetCore.SignalR.HubCallerContext hubCallerContext, out UserSession session)
        {
            return _users.TryGetValue(hubCallerContext.ConnectionId, out session);
        }

        public string[] GetUserConnections(string userId, string tokenId = null)
        {
            return _users.Where((x) => x.Value.Token.Subject == userId && (tokenId == null || x.Value.Token.Id != tokenId) && x.Value.Token.Payload.Exp > DateTime.Now.ToUnixTimestamp()).Select((x) => x.Key).ToArray();
        }

        public string[] GetUserConnections(string []userIds, string tokenId = null)
        {
            return _users.Where((x) => userIds.FirstOrDefault((s) => s == x.Value.Token.Subject) != null && (tokenId == null || x.Value.Token.Id != tokenId) && x.Value.Token.Payload.Exp > DateTime.Now.ToUnixTimestamp()).Select((x) => x.Key).ToArray();
        }

        private bool AddUserSession(string connectionId, UserSession session) {
            if (_users.TryAdd(connectionId, session))
            {
                //Console.WriteLine("Session Addded 1");
                OnConnected.Invoke(session);
                return true;
            }
            return false;
        }

        private bool RemoveUserSession(string connectionId, out UserSession session)
        {
            if (_users.TryRemove(connectionId, out session)) 
            { 
                OnDisconnected.Invoke(session);
                return true;
            }
            return false;
        }

        private bool RemoveUserSession(string connectionId)
        {
            UserSession session;
            return RemoveUserSession(connectionId, out session);
        }

        public void Connected(Microsoft.AspNetCore.SignalR.HubCallerContext hubCallerContext, string token)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var tokenService = scope.ServiceProvider.GetService<ITokenService>();
                JwtSecurityToken tkn = null;
                try
                {
                    //Console.WriteLine("Session Addded 3");
                    tkn = tokenService.ValidateToken(token);
                }
                catch (ClientException)
                {
                    return;
                }
                catch (Exception)
                {
                    return;
                }
                //Console.WriteLine("Session Addded 2");
                UserSession session;
                if (!_users.TryGetValue(hubCallerContext.ConnectionId, out session)) 
                   AddUserSession(hubCallerContext.ConnectionId, new UserSession(tkn, hubCallerContext.ConnectionId));
            }
        }

        public void Disconnected(Microsoft.AspNetCore.SignalR.HubCallerContext hubCallerContext)
        {
            RemoveUserSession(hubCallerContext.ConnectionId);
        }
    }
}

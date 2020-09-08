using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.SignalR.Hubs;
using SchoolBridge.Helpers.AddtionalClases.OnlineService;
using SchoolBridge.Helpers.AddtionalClases.UserConnectionService;
using SchoolBridge.Helpers.Extentions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class OnlineService: IOnlineService
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, OnlineStatusSubscription>> _allUsersUnderSupervision = new ConcurrentDictionary<string, ConcurrentDictionary<string, OnlineStatusSubscription>>();
        private readonly ConcurrentDictionary<string, OnlineStatusSubscriptionSession> _allSubscribedUsers = new ConcurrentDictionary<string, OnlineStatusSubscriptionSession>();

        private readonly IHubContext<ServerHub> _hubContext;
        private readonly IUserConnectionService _userConnectionService;
        private readonly OnlineServiceConfiguration _configuration;

        public OnlineService(IHubContext<ServerHub> hubContext,
                            IUserConnectionService userConnectionService,
                            OnlineServiceConfiguration configuration)
        {
            _hubContext = hubContext;
            _configuration = configuration;
            _userConnectionService = userConnectionService;

            _userConnectionService.OnConnected += OnConnected;
            _userConnectionService.OnDisconnected += OnDisconnected;
        }

        public OnlineStatus GetOnlineStatus(string userId) {
            bool found = false;
            _userConnectionService.ConnectedUsers.ForEach(x => { if (x.Value.UserId == userId) found = true; });
            if (found)
                return OnlineStatus.Online;
            return OnlineStatus.Offline;
        }

        public string CreateOnlineStatusSubscriptionToken(string creatorTokenId, string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, creatorTokenId),
                new Claim(JwtRegisteredClaimNames.Sub, userId),
            };

            var creds = new SigningCredentials(_configuration.OnlineTokenValidation.IssuerSigningKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.OnlineTokenValidation.ValidIssuer,
                claims: claims,
                signingCredentials: creds
            );

            return _configuration.TokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token, out JwtSecurityToken validatedToken)
        {
            SecurityToken validatedToken1;
            try
            {
                _configuration.TokenHandler.ValidateToken(token, _configuration.OnlineTokenValidation, out validatedToken1);
            }
            catch (Exception)
            {
                validatedToken = null;
                return false;
            }
            validatedToken = validatedToken1 as JwtSecurityToken;
            return true;
        }

        public void SubscribeToOnline(UserSession session, string subscribeToken)
        {
            JwtSecurityToken token;
            if (!ValidateToken(subscribeToken, out token))
                return;

            OnlineStatusSubscription subscription = new OnlineStatusSubscription(token);

            if (session.TokenId != subscription.Token.Id)
                return;

            if (!_allSubscribedUsers.ContainsKey(session.ConnectionId))
                _allSubscribedUsers.TryAdd(session.ConnectionId, new OnlineStatusSubscriptionSession(session, _allUsersUnderSupervision));

            _allSubscribedUsers[session.ConnectionId].AddUserToSupervision(subscription);
        }

        public void UnsubscribeToOnline(UserSession session, string subscribeToken)
        {
            if (_allSubscribedUsers.ContainsKey(session.ConnectionId))
                return;

            JwtSecurityToken token;
            if (!ValidateToken(subscribeToken, out token))
                return;

            OnlineStatusSubscriptionSession session1;
            _allSubscribedUsers.TryRemove(session.ConnectionId, out session1);
        }

        public async Task OnConnected(UserSession session) {
            if (!_allUsersUnderSupervision.ContainsKey(session.UserId))
                return;

            await _hubContext.Clients.Clients(_allUsersUnderSupervision[session.UserId].Keys.ToArray()).SendAsync("OnlineStatusCheck", session.UserId, OnlineStatus.Online);
        }

        public async Task OnDisconnected(UserSession session)
        {
            OnlineStatusSubscriptionSession session1;
            if (_allSubscribedUsers.TryRemove(session.ConnectionId, out session1)) 
                session1.ClearAllSubscriptions();

            if (!_allUsersUnderSupervision.ContainsKey(session.UserId))
                return;
            if (_userConnectionService.ConnectedUsers.Values.FirstOrDefault(x => x.UserId == session.UserId) != null)
                return;
            await _hubContext.Clients.Clients(_allUsersUnderSupervision[session.UserId].Keys.ToArray()).SendAsync("OnlineStatusCheck", session.UserId, OnlineStatus.Offline);
        }

        public void Dispose() {
            _userConnectionService.OnConnected -= OnConnected;
            _userConnectionService.OnDisconnected -= OnDisconnected;
        }
    }
}

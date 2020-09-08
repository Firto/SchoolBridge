using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.SignalR.Hubs;
using SchoolBridge.Helpers.AddtionalClases.ChatEventService;
using SchoolBridge.Helpers.AddtionalClases.ChatEventService.Events;
using SchoolBridge.Helpers.AddtionalClases.UserConnectionService;
using SchoolBridge.Helpers.DtoModels.Chat;
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
    public class ChatEventService : IChatEventService
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ChatEventSubscription>> _allChatsUnderSupervision = new ConcurrentDictionary<string, ConcurrentDictionary<string, ChatEventSubscription>>();
        private readonly ConcurrentDictionary<string, ChatEventSubscriptionSession> _allSubscribedUsers = new ConcurrentDictionary<string, ChatEventSubscriptionSession>();

        private readonly IHubContext<ServerHub> _hubContext;
        private readonly IUserConnectionService _userConnectionService;
        private readonly ChatEventServiceConfiguration _configuration;

        public ChatEventService(IHubContext<ServerHub> hubContext,
                            IUserConnectionService userConnectionService,
                            ChatEventServiceConfiguration configuration)
        {
            _hubContext = hubContext;
            _configuration = configuration;
            _userConnectionService = userConnectionService;

            _userConnectionService.OnDisconnected += OnDisconnected;
        }

        public string[] GetSubscribedClients(string chatId)
        {
            ConcurrentDictionary<string, ChatEventSubscription> conns;
            if (_allChatsUnderSupervision.TryGetValue(chatId, out conns))
                return conns.Select(x => x.Key).ToArray();
            else return new string[] { };
        }

        public string CreateSubscriptionToken(string creatorTokenId, string chatId)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, creatorTokenId),
                new Claim(JwtRegisteredClaimNames.Sub, chatId),
            };

            var creds = new SigningCredentials(_configuration.ChatEventTokenValidation.IssuerSigningKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.ChatEventTokenValidation.ValidIssuer,
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
                _configuration.TokenHandler.ValidateToken(token, _configuration.ChatEventTokenValidation, out validatedToken1);
            }
            catch (Exception)
            {
                validatedToken = null;
                return false;
            }
            validatedToken = validatedToken1 as JwtSecurityToken;
            return true;
        }

        public void Subscribe(UserSession session, string subscribeToken)
        {
            JwtSecurityToken token;
            if (!ValidateToken(subscribeToken, out token))
                return;

            ChatEventSubscription subscription = new ChatEventSubscription(token);

            if (session.TokenId != subscription.Token.Id)
                return;

            if (!_allSubscribedUsers.ContainsKey(session.ConnectionId))
                _allSubscribedUsers.TryAdd(session.ConnectionId, new ChatEventSubscriptionSession(session, _allChatsUnderSupervision));

            _allSubscribedUsers[session.ConnectionId].AddChatToSupervision(subscription);
        }

        public void Unsubscribe(UserSession session, string subscribeToken)
        {
            if (_allSubscribedUsers.ContainsKey(session.ConnectionId))
                return;

            JwtSecurityToken token;
            if (!ValidateToken(subscribeToken, out token))
                return;

            ChatEventSubscriptionSession session1;
            if (_allSubscribedUsers.TryGetValue(session.ConnectionId, out session1))
                session1.RemoveChatFromSupervision(token.Subject);
        }

        public async Task OnDisconnected(UserSession session)
        {
            ChatEventSubscriptionSession session1;
            if (_allSubscribedUsers.TryRemove(session.ConnectionId, out session1))
                session1.ClearAllSubscriptions();

            if (!_allChatsUnderSupervision.ContainsKey(session.UserId))
                return;
        }

        public async Task SendEventAsync(string chatId, string type, IChatEventSource source) {
            if (!_allChatsUnderSupervision.ContainsKey(chatId))
                return;

            await _hubContext.Clients.Clients(GetSubscribedClients(chatId)).SendAsync("ChatEvent", new ChatEventDto { Type = type, ChatId = chatId, Source = source });
        }

        public void Dispose()
        {
            _userConnectionService.OnDisconnected -= OnDisconnected;
        }
    }
}

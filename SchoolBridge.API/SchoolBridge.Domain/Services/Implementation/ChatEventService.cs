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
        private static readonly long _typingTime = 2; // 5 seconds 

        private readonly ChatsUnderSupervision _chats = new ChatsUnderSupervision();

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

        public bool IsTyping(string chatId, string userId)
            => _chats.Chats.ContainsKey(chatId) && _chats.Chats[chatId].Users.ContainsKey(userId) && _chats.Chats[chatId].Users[userId].LastType + _typingTime > DateTime.Now.ToUnixTimestamp();

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

            if (session.TokenId != token.Id)
                return;

            _chats.Subscribe(token.Subject, session);
        }

        public void Unsubscribe(UserSession session, string subscribeToken)
        {
            JwtSecurityToken token;
            if (!ValidateToken(subscribeToken, out token))
                return;

            if (session.TokenId != token.Id)
                return;

            _chats.Unsubscribe(token.Subject, session);
        }

        public async Task OnDisconnected(UserSession session)
        {
            if (_chats.Users.ContainsKey(session.ConnectionId))
                _chats.FullDisconnectUser(session);
        }

        public async Task SendEventAsync(string chatId, string type, IChatEventSource source, string[] exlude = null)
        {
            if (!_chats.Chats.ContainsKey(chatId))
                return;

            await _hubContext.Clients.Clients(_chats.Chats[chatId].GetSessions().Where(x => exlude == null || !exlude.Contains(x.ConnectionId)).Select(x => x.ConnectionId).ToArray()).SendAsync("ChatEvent", new ChatEventDto { Type = type, ChatId = chatId, Source = source });

        }

        public async Task SendNewMessageEvent(string userId, string chatId, NewMessageSource source)
        {
            if (_chats.Chats.ContainsKey(chatId) && _chats.Chats[chatId].Users.ContainsKey(userId)) {
                _chats.Chats[chatId].Users[userId].SetNotTyping();

                var now = DateTime.Now.ToUnixTimestamp();
                await SendEventAsync(chatId, "Typing",
                new ChatTypingEventSource()
                {
                    Typing = _chats.Chats[chatId].Users.Values
                        .Where(x => x.LastType + _typingTime > now)
                        .Select(x => x.UserId).ToArray()
                });
            }
            await SendEventAsync(chatId, "newMessage", source);
        }

        public async Task SendTypingEvent(UserSession session, string chatId)
        {
            if (!_chats.Users.ContainsKey(session.ConnectionId))
                return;
            var som = _chats.GetUser(session);

            if (!som.Chats.ContainsKey(chatId))
                return;

            som.Chats[chatId].Users[som.UserId].UpdateLastType();

            await SendEventAsync(chatId, "Typing",
                new ChatTypingEventSource()
                {
                    Typing = som.Chats[chatId].Users.Values
                        .Where(x => x.LastType + _typingTime > som.Chats[chatId].Users[som.UserId].LastType)
                        .Select(x => x.UserId).ToArray()
                });
        }

        public void Dispose()
        {
            _userConnectionService.OnDisconnected -= OnDisconnected;
        }
    }
}

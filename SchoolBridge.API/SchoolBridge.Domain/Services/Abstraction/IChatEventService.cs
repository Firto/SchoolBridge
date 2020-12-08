using SchoolBridge.Helpers.AddtionalClases.ChatEventService.Events;
using SchoolBridge.Helpers.AddtionalClases.UserConnectionService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IChatEventService : IOnInitService
    { 
        string CreateSubscriptionToken(string creatorTokenId, string chatId);

        // Events

        Task SendEventAsync(string chatId, string type, IChatEventSource source, string[] exlude = null);

        Task SendTypingEvent(UserSession session, string chatId);

        Task SendNewMessageEvent(string userId, string chatId, NewMessageSource source);
        bool IsTyping(string chatId, string userId);
        void Subscribe(UserSession session, string subscribeToken);
        void Unsubscribe(UserSession session, string subscribeToken);
    }
}

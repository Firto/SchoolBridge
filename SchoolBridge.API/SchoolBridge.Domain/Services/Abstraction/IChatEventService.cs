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
        string[] GetSubscribedClients(string chatId);
        string CreateSubscriptionToken(string creatorTokenId, string chatId);

        // Events

        Task SendEventAsync(string chatId, string type, IChatEventSource source);

        void Subscribe(UserSession session, string subscribeToken);
        void Unsubscribe(UserSession session, string subscribeToken);
    }
}

using SchoolBridge.Helpers.AddtionalClases.OnlineService;
using SchoolBridge.Helpers.AddtionalClases.UserConnectionService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IOnlineService: IMyService, IDisposable
    {
        string CreateOnlineStatusSubscriptionToken(string creatorTokenId, string userId);
        void SubscribeToOnline(UserSession session, string subscribeToken);
        OnlineStatus GetOnlineStatus(string userId);
        void UnsubscribeToOnline(UserSession session, string subscribeToken);
    }
}

using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SchoolBridge.Helpers.DtoModels;
using System;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface INotificationService<AUser> where AUser : AuthUser
    {
        Task Notify(string type, INotificationSource sourse);
        Task Notify(string conn, string type, INotificationSource sourse);
        Task Notify(string[] conn, string type, INotificationSource sourse);
        Task Notify(AUser usr, string type, INotificationSource sourse);
        Task Notify(AUser usr, string tokenId, string type, INotificationSource sourse);
        Task Notify(AUser[] usrs, string type, INotificationSource sourse);
        Task PermanentNotify(string tokenId, string type, INotificationSource sourse);
        Task PermanentNotify(string[] tokenIds, string type, INotificationSource sourse);

        Task Read(AUser usr, string last, int count);

        PermanentSubscribeDto CreatePermanentToken(TimeSpan? exp = null);

        void OnConnected(HubCallerContext hubCallerContext, string token);
        void OnPermanentConnected(HubCallerContext hubCallerContext, string token);
        void OnDisconnected(HubCallerContext hubCallerContext);
    }
}

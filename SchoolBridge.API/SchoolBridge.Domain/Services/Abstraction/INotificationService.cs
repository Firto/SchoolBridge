using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SchoolBridge.Helpers.DtoModels;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface INotificationService : IMyService
    {
        Task NotifyAsync(string type, INotificationSource sourse);
        Task NotifyAsync(string conn, string type, INotificationSource sourse);
        Task NotifyAsync(string[] conn, string type, INotificationSource sourse);
        Task NotifyAsync(string userId, INotificationSource sourse, string type);
        Task NotifyAsync(string userId, string tokenId, string type, INotificationSource sourse);
        Task NotifyAsync(string[] userIds, INotificationSource sourse, string type);
        Task PermanentNotifyAsync(string tokenId, string type, INotificationSource sourse);
        Task PermanentNotifyAsync(string[] tokenIds, string type, INotificationSource sourse);
    }
}

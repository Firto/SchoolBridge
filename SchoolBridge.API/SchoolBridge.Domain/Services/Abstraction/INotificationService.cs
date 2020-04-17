using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

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

        Task Read(AUser usr, string last, int count);

        Task OnConnected(HubCallerContext hubCallerContext, string token);
        Task OnDisconnected(HubCallerContext hubCallerContext);
    }
}

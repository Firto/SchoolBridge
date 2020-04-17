using SchoolBridge.Helpers.DtoModels;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.SignalR.Clients
{
    public interface INotificationClient
    {
        Task Notification(NotificationDto ntf);
        Task Subscribed(string Id);
    }
}
using System.Threading.Tasks;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<ServerHub> _hubContext;
        private readonly IUserConnectionService _userConnectionService;
        private readonly IPermanentConnectionService _permanentConnectionService;

        private readonly string methodNotificationName = "Notification";

        public NotificationService(IHubContext<ServerHub> hubContext,
                                    IUserConnectionService userConnectionService,
                                    IPermanentConnectionService permanentConnectionService) {
            _hubContext = hubContext;
            _userConnectionService = userConnectionService;
            _permanentConnectionService = permanentConnectionService;
        }

        public async Task NotifyAsync(string type, INotificationSource sourse)
           => await _hubContext.Clients.All.SendAsync(methodNotificationName, new NotificationDto {
               Type = type,
               Source = sourse
           });
        public async Task NotifyAsync(string conn, string type, INotificationSource sourse)
            => await _hubContext.Clients.Client(conn).SendAsync(methodNotificationName, new NotificationDto
            {
                Type = type,
                Source = sourse
            });

        public async Task NotifyAsync(string[] conns, string type, INotificationSource sourse)
            => await _hubContext.Clients.Clients(conns).SendAsync(methodNotificationName, new NotificationDto
            {
                Type = type,
                Source = sourse
            });

        public async Task NotifyAsync(string userId, INotificationSource sourse, string type)
            => await NotifyAsync(_userConnectionService.GetUserConnections(userId), type, sourse);

        public async Task NotifyAsync(string userId, string tokenId, string type, INotificationSource sourse)
           => await NotifyAsync(_userConnectionService.GetUserConnections(userId, tokenId), type, sourse);

        public async Task NotifyAsync(string[] userIds, INotificationSource sourse, string type)
            => await NotifyAsync(_userConnectionService.GetUserConnections(userIds), type, sourse);

        public async Task PermanentNotifyAsync(string tokenId, string type, INotificationSource sourse)
            => await NotifyAsync(_permanentConnectionService.GetConnections(tokenId), type, sourse);

        public async Task PermanentNotifyAsync(string[] tokenIds, string type, INotificationSource sourse)
           => await NotifyAsync(_permanentConnectionService.GetConnections(tokenIds), type, sourse);

    }
}

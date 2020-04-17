using SchoolBridge.Domain.SignalR.Clients;
using SchoolBridge.Domain.Services.Abstraction;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using SchoolBridge.DataAccess.Entities.Authorization;

namespace SchoolBridge.Domain.SignalR.Hubs
{
    public class NotificationHub<AUser> : Hub<INotificationClient> where AUser : AuthUser
    {
        private readonly INotificationService<AUser> _notificationService;
        public NotificationHub(INotificationService<AUser> notificationService) {
            _notificationService = notificationService;
        }

        public async Task Subscribe(string token)
        {
            await _notificationService.OnConnected(Context, token);
        }
        public async Task UnSubscribe()
        {
            await _notificationService.OnDisconnected(Context);
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await _notificationService.OnDisconnected(Context);
            await base.OnDisconnectedAsync(ex);
        }
    }
}

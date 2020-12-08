using Microsoft.AspNetCore.SignalR;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.SignalR.Hubs;
using SchoolBridge.Helpers.AddtionalClases.DataBaseNotoficationService;
using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class BanUserService: IBanUsserService
    {
        private readonly IUserService _userService;
        private readonly IHubContext<ServerHub> _hubContext;
        private readonly IUserConnectionService _userConnectionService;
        private readonly IDataBaseNotificationService _notificationService;
        public BanUserService(IUserService userService,
                              IHubContext<ServerHub> hubContext,
                              IUserConnectionService userConnectionService,
                              IDataBaseNotificationService notificationService)
        {
            _userService = userService;
            _hubContext = hubContext;
            _userConnectionService = userConnectionService;
            _notificationService = notificationService;
        }

        public static void OnInit(IValidatingService validatingService)
        {

            validatingService.AddValidateFunc("str-reason", (string prop, PropValidateContext context) =>
            {
                if (prop == null) return;
                if (prop.Length > 300)
                    context.Valid.Add($"[str-too-long, [pn-{context.PropName}], 300]");// $"Please input {context.PropName}!"
            });
        }

        public async Task BanAsync(string userId, string reason)
        {
            await _userService.BanAsync(new User { Id = userId }, reason);
            await _notificationService.NotifyAsync(userId, "Ban", new BanNotificationSource() { Reason = reason });
            await _hubContext.Clients.Clients(_userConnectionService.GetUserConnections(userId)).SendAsync("ban", reason);
        }

        public void BanUser(string userId, string reason)
        {
            _userService.Ban(new User { Id = userId }, reason);
            _hubContext.Clients.Clients(_userConnectionService.GetUserConnections(userId)).SendAsync("ban", reason);
        }

        public async Task UnbanAsync(string userId)
        {
            await _userService.UnbanAsync(new User { Id = userId });
            await _notificationService.NotifyAsync(userId, "Unban", new NullNotificationSource());
        }

        public void UnbanUser(string userId)
        {
            _userService.Unban(new User { Id = userId });
            _notificationService.NotifyAsync(userId, "Unban", new NullNotificationSource());
        }
    }
}

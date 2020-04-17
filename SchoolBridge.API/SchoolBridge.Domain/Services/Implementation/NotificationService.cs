using System.Threading.Tasks;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.SignalR.Clients;
using SchoolBridge.Domain.SignalR.Hubs;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;
using Microsoft.AspNetCore.SignalR;
using SchoolBridge.DataAccess.Entities.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Concurrent;
using System.Linq;
using System;
using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class NotificationService<AUser> : INotificationService<AUser> where AUser : AuthUser
    {
        private readonly IHubContext<NotificationHub<AUser>, INotificationClient> _hubContext;
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<string, JwtSecurityToken> _users = new ConcurrentDictionary<string, JwtSecurityToken>();

        public NotificationService(IHubContext<NotificationHub<AUser>, INotificationClient> hubContext,
                                    IServiceProvider serviceProvider) {
            _hubContext = hubContext;
            _serviceProvider = serviceProvider;
        }

        private string[] GetConns(AUser usr, string tokenId = null)
        {
            return _users.Where((x) => x.Value.Subject == usr.Id && (tokenId == null || x.Value.Id != tokenId) && x.Value.Payload.Exp > DateTime.Now.ToUnixTimestamp()).Select((x) => x.Key).ToArray();
        }

        private string[] GetConns(AUser[] usr, string tokenId = null)
        {
            return _users.Where((x) => usr.FirstOrDefault((s) => s.Id == x.Value.Subject) != null && (tokenId == null || x.Value.Id != tokenId) && x.Value.Payload.Exp > DateTime.Now.ToUnixTimestamp()).Select((x) => x.Key).ToArray();
        }

        public async Task OnConnected(HubCallerContext hubCallerContext, string token)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var tokenService = scope.ServiceProvider.GetService<ITokenService<AUser>>();
                JwtSecurityToken tkn = null;
                try
                {
                    tkn = tokenService.ValidateToken(token);
                }
                catch (ClientException) {
                    return;
                }
                catch (Exception) {
                    return;
                }
                _users.AddOrUpdate(hubCallerContext.ConnectionId, tkn, (x, s) => tkn);
            }
        }

        public async Task OnDisconnected(HubCallerContext hubCallerContext)
            => _users.TryRemove(hubCallerContext.ConnectionId, out var sor);

        public async Task Notify(string type, INotificationSource sourse)
           => await _hubContext.Clients.All.Notification(new NotificationDto {
               Type = type,
               Source = sourse
           });
        public async Task Notify(string conn, string type, INotificationSource sourse)
            => await _hubContext.Clients.Client(conn).Notification(new NotificationDto
            {
                Type = type,
                Source = sourse
            });

        public async Task Notify(string[] conns, string type, INotificationSource sourse)
            => await _hubContext.Clients.Clients(conns).Notification(new NotificationDto
            {
                Type = type,
                Source = sourse
            });

        public async Task Notify(AUser usr, string type, INotificationSource sourse)
            => await Notify(GetConns(usr), type, sourse);

        public async Task Notify(AUser usr, string tokenId, string type, INotificationSource sourse)
           => await Notify(GetConns(usr, tokenId), type, sourse);

        public async Task Notify(AUser[] usrs, string type, INotificationSource sourse)
            => await Notify(GetConns(usrs), type, sourse);

        public async Task Read(AUser usr, string last, int count)
            => await _hubContext.Clients.Clients(GetConns(usr)).Notification(
                    new NotificationDto
                    {
                        Type = "onReadNtf",
                        Source = new OnReadSource { Last = last, Count = count }
                    }
                );
    }
}

using SchoolBridge.Domain.Services.Abstraction;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.Helpers.AddtionalClases.UserConnectionService;

namespace SchoolBridge.Domain.SignalR.Hubs
{
    public class ServerHub : Hub 
    {
        private readonly IUserConnectionService _userConnectionService;
        private readonly IPermanentConnectionService _permanentConnectionService;
        private readonly IOnlineService _onlineService;
        private readonly IChatEventService _chatEventService;
        public ServerHub(IUserConnectionService userConnectionService,
                        IPermanentConnectionService permanentConnectionService,
                        IOnlineService onlineService,
                        IChatEventService chatEventService) {
            _userConnectionService = userConnectionService;
            _permanentConnectionService = permanentConnectionService;
            _onlineService = onlineService;
            _chatEventService = chatEventService;
        }

        public void Subscribe(string token)
        {   
            _userConnectionService.Connected(Context, token);
        }

        public void PermanentSubscribe(string token)
        {
            _permanentConnectionService.OnConnected(Context, token);
        }

        public void UnSubscribe()
        {
            _userConnectionService.Disconnected(Context);
            _permanentConnectionService.OnDisconnected(Context);
        }

        public void OnlineSubscribe(string token)
        {
            UserSession session;
            if (!_userConnectionService.GetUserSession(Context, out session)) return;
            _onlineService.SubscribeToOnline(session, token);
        }

        public void ChatSubscribe(string token)
        {
            UserSession session;
            if (!_userConnectionService.GetUserSession(Context, out session)) return;
            _chatEventService.Subscribe(session, token);
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            _userConnectionService.Disconnected(Context);
            _permanentConnectionService.OnDisconnected(Context);
            await base.OnDisconnectedAsync(ex);
        }
    }
}

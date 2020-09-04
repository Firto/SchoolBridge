using Microsoft.AspNetCore.SignalR;
using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.Helpers.AddtionalClases.UserConnectionService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IUserConnectionService
    {
        IDictionary<string, UserSession> ConnectedUsers { get; }
        event UserConnectionEvent OnConnected;
        event UserConnectionEvent OnDisconnected;

        void UpdateUserToken(string oldTokenId, string newToken);

        string[] GetUserConnections(string usrId, string tokenId = null);
        string[] GetUserConnections(string[] usrId, string tokenId = null);

        UserSession GetUserSession(Microsoft.AspNetCore.SignalR.HubCallerContext hubCallerContext);

        bool GetUserSession(Microsoft.AspNetCore.SignalR.HubCallerContext hubCallerContext, out UserSession session);

        void Connected(HubCallerContext hubCallerContext, string token);
        void Disconnected(HubCallerContext hubCallerContext);
    }
}

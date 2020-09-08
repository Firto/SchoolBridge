
using Microsoft.AspNetCore.SignalR;
using SchoolBridge.Helpers.DtoModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IPermanentConnectionService: IOnInitService
    {
        IDictionary<string, JwtSecurityToken> ConnectedUsers { get; }

        PermanentSubscribeDto CreatePermanentToken(TimeSpan? exp, out string guid);
        JwtSecurityToken ValidatePermanentToken(string token);

        string[] GetConnections(string tokenId);
        string[] GetConnections(string[] tokenIds);

        void OnConnected(HubCallerContext hubCallerContext, string token);
        void OnDisconnected(HubCallerContext hubCallerContext);
    }
}

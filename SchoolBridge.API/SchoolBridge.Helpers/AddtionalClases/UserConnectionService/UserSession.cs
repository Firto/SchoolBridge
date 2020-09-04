using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SchoolBridge.Helpers.AddtionalClases.UserConnectionService
{
    public class UserSession
    {
        public JwtSecurityToken Token { get; private set; }

        public string ConnectionId { get; private set; }
        public string UserId { get; private set; }
        public string TokenId { get; private set; }
        
        public UserSession(JwtSecurityToken token, string connectionId) {
            ConnectionId = connectionId;
            Token = token;
            TokenId = Token.Id;
            UserId = Token.Subject;
        }

        public void UpdateToken(JwtSecurityToken token) {
            Token = token;
            TokenId = Token.Id;
        }
    }
}

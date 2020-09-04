using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SchoolBridge.Helpers.AddtionalClases.OnlineService
{
    public class OnlineStatusSubscription: IEquatable<OnlineStatusSubscription>
    {
        public JwtSecurityToken Token { get; private set; }
        public string ClientUnderSupervisionId { get; private set; }

        public OnlineStatusSubscriptionSession Session { get; set; }

        public OnlineStatusSubscription(JwtSecurityToken token) {
            Token = token;
            ClientUnderSupervisionId = token.Subject;
        }

        public bool Equals(OnlineStatusSubscription other)
        {
            // First two lines are just optimizations
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Session.ClientSession.ConnectionId.Equals(other.Session.ClientSession.ConnectionId);
        }

        public override bool Equals(object obj)
        {
            // Again just optimization
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            // Actually check the type, should not throw exception from Equals override
            if (obj.GetType() != this.GetType()) return false;

            // Call the implementation from IEquatable
            return Equals((OnlineStatusSubscription)obj);
        }

        public override int GetHashCode()
        {
            // Constant because equals tests mutable member.
            // This will give poor hash performance, but will prevent bugs.
            return 0;
        }
    }
}

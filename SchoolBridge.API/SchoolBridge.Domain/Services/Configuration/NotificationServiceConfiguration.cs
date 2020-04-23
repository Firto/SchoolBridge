using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class NotificationServiceConfiguration
    {
        public SecurityTokenHandler TokenHandler { get; set; } = new JwtSecurityTokenHandler();
        public TokenValidationParameters PermanentTokenValidation { get; set; }
        public TimeSpan PermanentTokenExpires { get; set; }
    }
}

using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using SchoolBridge.Domain.Services.Abstraction;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class PermanentConnectionServiceConfiguration: IMyService
    {
        public SecurityTokenHandler TokenHandler { get; set; } = new JwtSecurityTokenHandler();
        public TokenValidationParameters PermanentTokenValidation { get; set; }
        public TimeSpan PermanentTokenExpires { get; set; }
    }
}

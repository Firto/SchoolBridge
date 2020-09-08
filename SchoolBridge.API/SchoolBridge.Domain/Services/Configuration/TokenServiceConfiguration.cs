using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using SchoolBridge.Domain.Services.Abstraction;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class TokenServiceConfiguration: IMyService
    {
        public SecurityTokenHandler TokenHandler { get; set; } = new JwtSecurityTokenHandler();

        public TokenValidationParameters RefreshTokenValidation { get; set; }
        public TokenValidationParameters TokenValidation { get; set; }

        public TimeSpan TokenExpires { get; set; }
        public TimeSpan RefreshTokenExpires { get; set; }
        public TimeSpan RefreshTokenRemove { get; set; }
    }
}

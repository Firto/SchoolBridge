using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class TokenServiceConfiguration
    {
        public SecurityTokenHandler TokenHandler { get; set; } = new JwtSecurityTokenHandler();

        public TokenValidationParameters RefreshTokenValidation { get; set; }
        public TokenValidationParameters TokenValidation { get; set; }

        public TimeSpan TokenExpires { get; set; }
        public TimeSpan RefreshTokenExpires { get; set; }
        public TimeSpan RefreshTokenRemove { get; set; }
    }
}

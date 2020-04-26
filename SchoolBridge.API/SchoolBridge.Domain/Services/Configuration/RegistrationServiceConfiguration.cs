using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class RegistrationServiceConfiguration
    {
        public SecurityTokenHandler TokenHandler { get; set; } = new JwtSecurityTokenHandler();
        public TokenValidationParameters RegistrationTokenValidation { get; set; }
        public TimeSpan RegistrationTokenExpires { get; set; }
    }
}

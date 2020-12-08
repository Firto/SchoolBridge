using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using SchoolBridge.Domain.Services.Abstraction;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class RegistrationServiceConfiguration: IMyService
    {
        public SecurityTokenHandler TokenHandler { get; set; } = new JwtSecurityTokenHandler();
        public TokenValidationParameters RegistrationTokenValidation { get; set; }
        public TimeSpan RegistrationTokenExpires { get; set; }
        public TimeSpan ChangePasswordTokenExpires { get; set; }

        public short MaxCountCharsLogin { get; set; } = 25;
        public short MaxCountCharsPassword { get; set; } = 25;
        public short MaxCountCharsName { get; set; } = 60;
        public short MaxCountCharsSurname { get; set; } = 255;
        public short MaxCountCharsLastname { get; set; } = 70;
        public short MinCountCharsPassword { get; set; } = 8;
    }
}

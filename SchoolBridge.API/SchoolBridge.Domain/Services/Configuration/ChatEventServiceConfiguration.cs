﻿using Microsoft.IdentityModel.Tokens;
using SchoolBridge.DataAccess.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class ChatEventServiceConfiguration
    {
        public SecurityTokenHandler TokenHandler { get; private set; } = new JwtSecurityTokenHandler();
        public TokenValidationParameters ChatEventTokenValidation { get; set; }
    }
}
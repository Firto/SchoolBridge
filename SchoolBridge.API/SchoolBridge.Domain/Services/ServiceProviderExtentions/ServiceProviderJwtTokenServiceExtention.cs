using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.Domain.Hostings;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;
using System;
using System.Text;

namespace SchoolBridge.Domain.Services.ServiceProviderExtentions
{
    public static class ServiceProviderJwtTokenServiceExtention
    {
        public static void UseJwtTokenService(this IServiceCollection services, TokenServiceConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddHostedService<TokenRemoveHosting>();
        }

        public static void UseJwtTokenService(this IServiceCollection services, IConfiguration configuration) 
        { 
            UseJwtTokenService(services, new TokenServiceConfiguration
            {
                TokenValidation = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = "JwtTokenService",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Key"])),
                    ClockSkew = TimeSpan.Zero
                },
                RefreshTokenValidation = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = "JwtTokenService",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["RefreshKey"])),
                    ClockSkew = TimeSpan.Zero
                },
                TokenExpires = TimeSpan.FromMinutes(int.Parse(configuration["ExpireMinuts"])),
                RefreshTokenExpires = TimeSpan.FromDays(int.Parse(configuration["RefreshExpireDays"])),
                RefreshTokenRemove = TimeSpan.FromDays(int.Parse(configuration["RefreshRemoveDays"]))
            });
        }
    }
}

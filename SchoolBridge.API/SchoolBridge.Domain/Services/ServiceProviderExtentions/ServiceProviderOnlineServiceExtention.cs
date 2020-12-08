using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;
using System;
using System.Text;

namespace SchoolBridge.Domain.Services.ServiceProviderExtentions
{
    public static class ServiceProviderOnlineServiceExtention
    {
        public static void UseOnlineService(this IServiceCollection services, OnlineServiceConfiguration configuration) 
        {
            services.AddSingleton(configuration);
            services.AddSingleton<IOnlineService, OnlineService>();
        }

        public static void UseOnlineService(this IServiceCollection services, IConfiguration configuration)
        {
            UseOnlineService(services, new OnlineServiceConfiguration
            {
                OnlineTokenValidation = new TokenValidationParameters
                {
                    ValidateLifetime = false,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = "OnlineService",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["OnlineKey"])),
                    ClockSkew = TimeSpan.Zero
                },
            });
        }
    }
}

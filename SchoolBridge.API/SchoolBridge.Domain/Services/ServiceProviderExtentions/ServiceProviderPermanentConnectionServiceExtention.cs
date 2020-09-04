using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;
using System;
using System.Text;

namespace SchoolBridge.Domain.Services.ServiceProviderExtentions
{
    public static class ServiceProviderPermanentConnectionServiceExtention
    {
        public static void UsePermanentConnectionService(this IServiceCollection services, PermanentConnectionServiceConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddSingleton<IPermanentConnectionService, PermanentConnectionService>();
        }

        public static void UsePermanentConnectionService(this IServiceCollection services, IConfiguration configuration)
        {
            UsePermanentConnectionService(services, new PermanentConnectionServiceConfiguration
            {
                PermanentTokenValidation = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = "PermanentConnectionService",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["PermanentKey"])),
                    ClockSkew = TimeSpan.Zero
                },
                PermanentTokenExpires = TimeSpan.FromMinutes(int.Parse(configuration["PermanentExpireMinuts"]))
            });
        }
    }
}

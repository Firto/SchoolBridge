using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Domain.Services.ServiceProviderExtentions
{
    public static class ServiceProviderLoginServiceExtention
    {
        public static void UseLoginRegistrationService(this IServiceCollection services, RegistrationServiceConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
        }

        public static void UseLoginRegistrationService(this IServiceCollection services, IConfiguration configuration)
        {
            UseLoginRegistrationService(services, new RegistrationServiceConfiguration {
                RegistrationTokenValidation = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = "RegistrationService",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["RegistrationKey"])),
                    ClockSkew = TimeSpan.Zero
                },
                RegistrationTokenExpires = TimeSpan.FromDays(int.Parse(configuration["RegistrationExpireDays"]))
            });
        }
    }
}

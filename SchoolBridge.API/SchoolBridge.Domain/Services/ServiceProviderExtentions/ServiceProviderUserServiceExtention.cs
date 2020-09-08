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
    public static class ServiceProviderUserServiceExtention
    {
        public static void UseUserService(this IServiceCollection services, UserServiceConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddScoped<IUserService, UserService>();
        }

        public static void UseUserService(this IServiceCollection services, IConfiguration configuration)
        {
            UseUserService(services, new UserServiceConfiguration
            {
                UserGetTokenValidation = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = "UserService",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["GetUserKey"])),
                    ClockSkew = TimeSpan.Zero
                },
            });
        }
    }
}

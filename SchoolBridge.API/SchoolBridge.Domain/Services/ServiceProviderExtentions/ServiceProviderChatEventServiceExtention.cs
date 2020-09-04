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
    public static class ServiceProviderChatEventServiceExtention
    {
        public static void UseChatEventService(this IServiceCollection services, ChatEventServiceConfiguration configuration) 
        {
            services.AddSingleton(configuration);
            services.AddSingleton<IChatEventService, ChatEventService>();
        }

        public static void UseChatEventService(this IServiceCollection services, IConfiguration configuration)
        {
            UseChatEventService(services, new ChatEventServiceConfiguration
            {
                ChatEventTokenValidation = new TokenValidationParameters
                {
                    ValidateLifetime = false,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = "ChatEventService",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["ChatEventKey"])),
                    ClockSkew = TimeSpan.Zero
                },
            });
        }
    }
}

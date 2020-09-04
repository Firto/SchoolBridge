using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Domain.Services.ServiceProviderExtentions
{
    public static class ServiceProviderLanguageStringServiceExtention
    {
        public static void UseLanguageStringService(this IServiceCollection services, LanguageStringServiceConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddScoped<ILanguageStringService, LanguageStringService>();
        }
    }
}

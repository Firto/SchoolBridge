using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.Domain.Hostings;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;

namespace SchoolBridge.Domain.Services.ServiceProviderExtentions
{
    public static class ServiceProviderEmailServiceExtention
    {
        public static void UseEmailService(this IServiceCollection services, EmailServiceConfiguration configuration) 
        {
            services.AddSingleton(configuration);
            services.AddSingleton<IEmailService, EmailService>();
            services.AddHostedService<EmailServiceHosting>();
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchoolBridge.Domain.Hostings;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;
using System;

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

        public static void UseEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            UseEmailService(services, new EmailServiceConfiguration
            {
                DraftsPath = "./" + configuration["DraftsPath"],
                EmailServersConfigPath = "./" + configuration["EmailServersConfigPath"],
                //SendEmailInterval = TimeSpan.FromMinutes(_emailServiceConfiguration.GetValue<uint>("SendEmailIntervalMinuts")),
                SendEmailInterval = TimeSpan.FromSeconds(10),
                MaxSendEmailInOneThread = configuration.GetValue<uint>("MaxSendEmailInOneThread"),
                MaxSendThreads = configuration.GetValue<uint>("MaxSendThreads"),
                DefaultSendFrom = configuration["DefaultSendFrom"],
            });
        }
    }
}

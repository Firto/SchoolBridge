using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;

namespace SchoolBridge.Domain.Services.ServiceProviderExtentions
{
    public static class ServiceProviderRegistrationServiceExtention
    {
        public static void UseRegistrationService(this IServiceCollection services, RegistrationServiceConfiguration configuration) 
        {
            services.AddSingleton(configuration);
            services.AddScoped<IRegistrationService, RegistrationService>();
        }
    }
}

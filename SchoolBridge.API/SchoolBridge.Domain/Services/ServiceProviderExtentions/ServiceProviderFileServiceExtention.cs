using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.Domain.Hostings;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;

namespace SchoolBridge.Domain.Services.ServiceProviderExtentions
{
    public static class ServiceProviderFileServiceExtention
    {
        public static void UseFileService(this IServiceCollection services, FileServiceConfiguration configuration) 
        {
            services.AddSingleton(configuration);
            services.AddScoped<IFileService, FileService>();
        }
    }
}

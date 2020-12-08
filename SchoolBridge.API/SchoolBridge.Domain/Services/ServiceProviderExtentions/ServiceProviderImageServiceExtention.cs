using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;

namespace SchoolBridge.Domain.Services.ServiceProviderExtentions
{
    public static class ServiceProviderImageServiceExtention
    {
        public static void UseImageService(this IServiceCollection services, ImageServiceConfiguration configuration) 
        {
            services.AddSingleton(configuration);
            services.AddScoped<IImageService, ImageService>();
        }
    }
}

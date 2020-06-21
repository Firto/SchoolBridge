using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;

namespace SchoolBridge.Domain.Services.ServiceProviderExtentions
{
    public static class ServiceProviderNotificationServiceExtention
    {
        public static void UseNotificationService(this IServiceCollection services, NotificationServiceConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddSingleton(typeof(INotificationService<>), typeof(NotificationService<>));
            services.AddScoped(typeof(IDataBaseNotificationService<>), typeof(DataBaseNotificationService<>));
        }
    }
}

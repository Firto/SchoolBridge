using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;

namespace SchoolBridge.Domain.Services.ServiceProviderExtentions
{
    public static class ServiceProviderProgramStatusServiceExtention
    {
        public static void UseProgramStatusService(this IServiceCollection services, ProgramStatusServiceConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddSingleton<IProgramStatusService, ProgramStatusService>();
        }
    }
}

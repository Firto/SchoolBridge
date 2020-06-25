using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Domain.Services.ServiceProviderExtentions
{
    public static class ServiceProviderSubjectServiceExtention
    {
        public static void UseSubjectService(this IServiceCollection services, SubjectServiceConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddScoped<ISubjectService, SubjectService>();
        }
    }
}

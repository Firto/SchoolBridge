﻿using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.Domain.Managers.CClientErrorManager;

namespace SchoolBridge.Domain.Services.ServiceProviderExtentions
{
    public static class ServiceProviderClientErrorManagerExtention
    {
        public static void UseClientErrorManager(this IServiceCollection services) 
        {
            services.AddSingleton<ClientErrorManager>();
        }
    }
}

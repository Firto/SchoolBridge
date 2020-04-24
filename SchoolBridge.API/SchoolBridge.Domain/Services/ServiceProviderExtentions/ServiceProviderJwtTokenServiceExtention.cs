using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.Domain.Hostings;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;

namespace SchoolBridge.Domain.Services.ServiceProviderExtentions
{
    public static class ServiceProviderJwtTokenServiceExtention
    {
        public static void UseJwtTokenService<AUser>(this IServiceCollection services, TokenServiceConfiguration configuration) where AUser : AuthUser
        {
            services.AddSingleton(configuration);
            services.AddScoped(typeof(ITokenService<>), typeof(JwtTokenService<>));
            services.AddHostedService<TokenRemoveHosting<AUser>> ();
        }
    }
}

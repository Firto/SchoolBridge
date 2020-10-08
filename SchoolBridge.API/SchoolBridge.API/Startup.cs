using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SchoolBridge.DataAccess;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.DataAccess.Repositories;
using SchoolBridge.Domain.Hostings;
using SchoolBridge.Domain.Profiles;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;
using SchoolBridge.Domain.Services.ServiceProviderExtentions;
using SchoolBridge.Domain.SignalR.Hubs;
using SchoolBridge.Helpers.Extentions;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;
using SchoolBridge.Helpers.AddtionalClases.ProgramStatusService;

namespace SchoolBridge.API
{

    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        //private readonly IEnumerable<string> _allowedOrgins = new string[] { "http://192.168.0.4:4200", "http://192.168.0.4:4300" };

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Initialization

            services.AddHostedService<ServiceInitializationHosting>();

            // Db Context
            services.AddDbContext<DbContext, ApplicationContext>(opt =>
                opt.UseNpgsql(_configuration.GetSection("ConnectionStrings")["mypostgres"]), optionsLifetime: ServiceLifetime.Singleton);

            // Mapper
            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainMapperProfile());
            }).CreateMapper());

            services.AddSingleton<IJsonConverterService, JsonConverterService>();

            // Program status

            services.UseProgramStatusService(new ProgramStatusServiceConfiguration
            {
                CurrentStatusPath = "./Assets/ProgramStatus.json",
                DefaultStatus = new ProgramStatus
                {
                    IsLoadedFirst = true
                }
            });

            // Client errors
            services.UseClientErrorManager();

            // Signalr notification

            services.AddSingleton<IUserConnectionService, UserConnectionService>();
            services.AddSingleton<INotificationService, NotificationService>();

            services.UsePermanentConnectionService(_configuration.GetSection("PermanentConnectionService"));
            services.UseOnlineService(_configuration.GetSection("OnlineService"));
            services.UseChatEventService(_configuration.GetSection("ChatEventService"));
            services.AddScoped<IDataBaseNotificationService, DataBaseNotificationService>();

            // File service
            services.UseFileService(new FileServiceConfiguration
            {
                SaveDirectory = _env.ContentRootPath + "/Assets/Files"
            });

            // Image Service
            services.UseImageService(new ImageServiceConfiguration());

            // Email Service
            services.UseEmailService(_configuration.GetSection("EmailService"));

            // Custom jwt auth
            services.UseJwtTokenService(_configuration.GetSection("JwtSettings"));

            // Scoped Services
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ScopedHttpContext>();
            services.AddScoped<IValidatingService, ValidatingService>();

            services.AddScoped<ILanguageStringService, LanguageStringService>();
            services.AddScoped<IRoleService, RoleService>();
            services.UseUserService(_configuration.GetSection("UserService"));
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IDirectMessagesService, DirectMessageService>();

            services.UseSubjectService(new SubjectServiceConfiguration());
            services.UseLoginRegistrationService(_configuration.GetSection("RegistrationService"));
            services.UseLanguageStringService(new LanguageStringServiceConfiguration { DefaultLanguage = "en" });
            //SignalR
            services.AddSignalR().AddNewtonsoftJsonProtocol().AddHubOptions<ServerHub>(options =>
            {
                options.EnableDetailedErrors = true;
            });
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x.SetIsOriginAllowed(_ => true/*(x) => { Console.WriteLine(x); return _allowedOrgins.Contains(x); }*/)
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST")
                            .AllowCredentials());

            app.UseDeveloperExceptionPage();
            app.UseMiddleware<ScopedHttpContextMiddleware>();
            app.UseMiddleware<ClientErrorsMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ServerHub>("/api/notify");
            });
        }
    }
}

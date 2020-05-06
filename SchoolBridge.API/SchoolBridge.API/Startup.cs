using System;
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
using SchoolBridge.Domain.Profiles;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;
using SchoolBridge.Domain.Services.ServiceProviderExtentions;
using SchoolBridge.Domain.SignalR.Hubs;
using SchoolBridge.Helpers.Extentions;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;

namespace SchoolBridge.API
{

    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IConfiguration _jwtConfiguration;
        private readonly IConfiguration _emailServiceConfiguration;
        private readonly IConfiguration _notificationServiceConfiguration;
        private readonly IConfiguration _registrationServiceConfiguration;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _jwtConfiguration = _configuration.GetSection("JwtSettings");
            _emailServiceConfiguration = _configuration.GetSection("EmailService");
            _notificationServiceConfiguration = _configuration.GetSection("NotificationService");
            _registrationServiceConfiguration = _configuration.GetSection("RegistrationService");
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DbContext, ApplicationContext>(opt =>
                opt.UseNpgsql(_configuration.GetSection("ConnectionStrings")["mypostgres"]), optionsLifetime: ServiceLifetime.Singleton);

            services.UseClientErrorManager();
            services.UseJwtTokenService<User>(new TokenServiceConfiguration
            {
                TokenValidation = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = _jwtConfiguration["Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration["Key"])),
                    ClockSkew = TimeSpan.Zero
                },
                RefreshTokenValidation = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = _jwtConfiguration["Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration["RefreshKey"])),
                    ClockSkew = TimeSpan.Zero
                },
                TokenExpires = TimeSpan.FromMinutes(int.Parse(_jwtConfiguration["ExpireMinuts"])),
                RefreshTokenExpires = TimeSpan.FromDays(int.Parse(_jwtConfiguration["RefreshExpireDays"])),
                RefreshTokenRemove = TimeSpan.FromDays(int.Parse(_jwtConfiguration["RefreshRemoveDays"]))
            });
            services.UseNotificationService(new NotificationServiceConfiguration
            {
                PermanentTokenValidation = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = _jwtConfiguration["Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_notificationServiceConfiguration["PermanentKey"])),
                    ClockSkew = TimeSpan.Zero
                },
                PermanentTokenExpires = TimeSpan.FromMinutes(int.Parse(_notificationServiceConfiguration["PermanentExpireMinuts"]))
            });
            services.UseFileService(new FileServiceConfiguration
            {
                SaveDirectory = _env.ContentRootPath + "/Assets/Files"
            });
            services.UseImageService(new ImageServiceConfiguration());

            services.UseEmailService(new EmailServiceConfiguration
            {
                DraftsPath = _env.ContentRootPath + "/" + _emailServiceConfiguration["DraftsPath"],
                EmailServersConfigPath = _env.ContentRootPath + "/" + _emailServiceConfiguration["EmailServersConfigPath"],
                //SendEmailInterval = TimeSpan.FromMinutes(_emailServiceConfiguration.GetValue<uint>("SendEmailIntervalMinuts")),
                SendEmailInterval = TimeSpan.FromSeconds(10),
                MaxSendEmailInOneThread = _emailServiceConfiguration.GetValue<uint>("MaxSendEmailInOneThread"),
                MaxSendThreads = _emailServiceConfiguration.GetValue<uint>("MaxSendThreads"),
                DefaultSendFrom = _emailServiceConfiguration["DefaultSendFrom"],
            });

            services.UseRegistrationService(new RegistrationServiceConfiguration {
                RegistrationTokenValidation = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = _jwtConfiguration["Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_registrationServiceConfiguration["RegistrationKey"])),
                    ClockSkew = TimeSpan.Zero
                },
                RegistrationTokenExpires = TimeSpan.FromDays(int.Parse(_registrationServiceConfiguration["RegistrationExpireDays"]))
            });

            services.AddSingleton(new ValidatingServiceConfiguration { });

            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainMapperProfile());
            }).CreateMapper());

            // Scoped Services
            services.AddScoped<ScopedHttpContext>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IPanelService, PanelService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IValidatingService, ValidatingService>();
            services.AddScoped<IRegistrationService, RegistrationService>();

            services.AddScoped<ILoginService, LoginService>();

            //SignalR
            services.AddSignalR().AddNewtonsoftJsonProtocol();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x.WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST")
                            .AllowCredentials());
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseDeveloperExceptionPage();
            app.UseMiddleware<ScopedHttpContextMiddleware>();
            app.UseMiddleware<ClientErrorsMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub<User>>("/api/notify");
            });
        }
    }
}

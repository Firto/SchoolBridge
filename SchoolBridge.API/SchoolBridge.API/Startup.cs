using System;
using System.Text;
using AutoMapper;
using GreenP.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.DataAccess.Repositories;
using SchoolBridge.Domain.Hostings;
using SchoolBridge.Domain.Profiles;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Domain.Services.Implementation;
using SchoolBridge.Domain.SignalR.Hubs;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;

namespace SchoolBridge.API
{

    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IConfiguration _jwtConfiguration;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _jwtConfiguration = _configuration.GetSection("JwtSettings");
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DbContext, ApplicationContext>(opt =>
                opt.UseSqlServer(_configuration.GetSection("ConnectionStrings")["mymssql"]), optionsLifetime: ServiceLifetime.Singleton);

            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainMapperProfile());
            }).CreateMapper());

            // configuration

            services.AddSingleton(new TokenServiceConfiguration
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

            services.AddSingleton(new FileServiceConfiguration
            {
                SaveDirectory = _env.ContentRootPath + "/" + "Files"
            });

            services.AddSingleton(new ImageServiceConfiguration());

            // Singleton
            services.AddSingleton<ClientErrorManager>();
            services.AddSingleton(typeof(INotificationService<>), typeof(NotificationService<>));

            // Hosted Services
            services.AddHostedService<TokenRemoveHosting<User>>();

            // Scoped Services
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(ITokenService<>), typeof(JwtTokenService<>));
            services.AddScoped(typeof(IDataBaseNotificationService<>), typeof(DataBaseNotificationService<>));

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IImageService, ImageService>();

            //SignalR

            services.AddSignalR();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseDeveloperExceptionPage();
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

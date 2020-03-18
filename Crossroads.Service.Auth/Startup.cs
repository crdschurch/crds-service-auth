using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Crossroads.Web.Common.Configuration;
using Crossroads.Service.Auth.Services;
using Crossroads.Service.Auth.Interfaces;
using Crossroads.Microservice.Settings;
using Crossroads.Microservice.Logging;

namespace Crossroads.Service.Auth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "crds-service-auth", Version = "v1" });
            });

            string vaultRoleId = System.Environment.GetEnvironmentVariable("VAULT_ROLE_ID");
            string vaultSecretId = System.Environment.GetEnvironmentVariable("VAULT_SECRET_ID");
            SettingsService settingsService = new SettingsService(vaultRoleId, vaultSecretId);
            services.AddSingleton<ISettingsService>(settingsService);

            //Logging
            Logger.SetUpLogging(settingsService.GetSetting("LOGZIO_API_TOKEN"), settingsService.GetSetting("CRDS_ENV"));

            // Register all the webcommon stuff
            WebCommonSettingsConfig webCommonConfig = new WebCommonSettingsConfig(
                null,
                settingsService.GetSetting("MP_OAUTH_BASE_URL"),
                settingsService.GetSetting("MP_REST_API_ENDPOINT"),
                settingsService.GetSetting("CRDS_MP_COMMON_CLIENT_ID"),
                settingsService.GetSetting("CRDS_MP_COMMON_CLIENT_SECRET"),
                null,
                null
            );
            CrossroadsWebCommonConfig.Register(services, webCommonConfig);

            //Add services

            //Add singleton does not seem to actually add a singleton unless you create it and pass it in
            OIDConfigurationService configurationService = new OIDConfigurationService(settingsService);
            services.AddSingleton<IOIDConfigurationService>(configurationService);

            services.AddSingleton<IJwtService, JwtService>();
            services.AddSingleton<IMpUserService, MpUserService>();
            services.AddSingleton<IOktaUserService, OktaUserService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IIdentityService, IdentityService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "crds-service-auth");
                c.RoutePrefix = string.Empty;
            });

            // app.UseHttpsRedirection();

            app.UseMvc();
        }
    }
}

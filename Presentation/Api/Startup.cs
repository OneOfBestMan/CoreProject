using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Data;
using Model;
using Service;
using Core.ConfigReader;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Core.Filter;
using Core.Localization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Api
{
    /// <summary>
    /// Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Api.Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">Services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IConfig, JsonConfigRepository>();
            services.AddSingleton<ILocalization, JsonLocalizationRepository>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var config = services.BuildServiceProvider().GetService<IConfig>();

            var appMode = "Release";
#if DEBUG
            appMode = "Debug";
#endif

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = config.GetValue<string>("RedisConnectionString");
                options.InstanceName = $"{config.GetValue<string>("Name")}:{appMode}:";
            });

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", GetApiInfo(services));
                options.OperationFilter<SwaggerDefaultValues>();
                options.OperationFilter<AddRequiredHeaderParameter>();
                options.OperationFilter<FileUploadOperation>();
                options.IncludeXmlComments(XmlCommentsFilePath);
            });

            #region Repository
            services.AddSingleton<IRepository<Applications>, DapperRepository<Applications>>();
            #endregion

            #region Service
            services.AddSingleton<IApplicationService, ApplicationService>();
            #endregion
        }

        /// <summary>
        /// Configure the specified app and env.
        /// </summary>
        /// <param name="app">App.</param>
        /// <param name="env">Env.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                options.InjectStylesheet("/swagger-ui/custom.css");
            });

            app.UseCors(builder => builder.WithOrigins("http://*.netwa.app").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseCors(builder => builder.WithOrigins("https://*.netwa.app").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseCors(builder => builder.WithOrigins("http://*.whatsdog.net").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseCors(builder => builder.WithOrigins("https://*.whatsdog.net").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseStaticFiles();
            app.UseMvc();
        }

        /// <summary>
        /// Gets the api ınfo.
        /// </summary>
        /// <returns>The api ınfo.</returns>
        /// <param name="services">Services.</param>
        static Info GetApiInfo(IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfig>();
            var appMode = "Release";
#if DEBUG
            appMode = "Debug";
#endif

            var info = new Info
            {
                Title = config.GetValue<string>("Name"),
                Version = config.GetValue<string>("Version"),
                Description = $"<p>{config.GetValue<string>("Description")}</p><p>[BaseUrl : {config.GetValue<string>("BaseUrl")}] [<b>{appMode}</b>]</p>",
                Contact = new Contact { Name = config.GetValue<string>("CompanyName"), Email = config.GetValue<string>("SupportMail") }
            };

            return info;
        }

        /// <summary>
        /// Gets the xml comments file path.
        /// </summary>
        /// <value>The xml comments file path.</value>
        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
}
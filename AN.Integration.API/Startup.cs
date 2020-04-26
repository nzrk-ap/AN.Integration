using System;
using AN.Integration.API.Filters;
using AN.Integration.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AN.Integration.API
{
    public class Startup
    {
        public Startup(IHostEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables()
                .AddConfiguration(configuration);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(AuthorizationFilter));
                options.Filters.Add(new ConsumesAttribute("application/json"));
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddHttpClient();
            services.AddScoped<AuthorizationFilter>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient(provider =>
            {
                var serviceBusSasKey = Configuration.GetValue<string>("ApiExportQueueSasKey");
                var endpointUri = new Uri(Configuration.GetValue<string>("ApiExportQueueEndpoint"));
                return new HttpQueueClient(serviceBusSasKey, endpointUri);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
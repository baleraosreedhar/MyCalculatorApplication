using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.Logging.AzureAppServices;

namespace TechnicCharacterCountService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            // Configure the Serilog pipeline
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            // Add Serilog to the logging pipeline
            loggerFactory.AddSerilog();
            // will only be used for app services
            ////loggerFactory.AddAzureWebAppDiagnostics(
            ////            new AzureAppServicesDiagnosticsSettings
            ////            {
            ////                OutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss zzz} [{Level}] {RequestId}-{SourceContext}: {Message}{NewLine}{Exception}"
            ////            }
            ////        );
            app.UseMvc();
        }
    }
}

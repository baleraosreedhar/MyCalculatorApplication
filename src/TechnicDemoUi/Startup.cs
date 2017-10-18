using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Serilog;
using Microsoft.WindowsAzure.Storage;

namespace TechnicDemoUi
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
            Configuration = builder.Build();
           
            //var storageConnectionString=Configuration.GetValue<string>("ConnectionStrings:StorageConnection");
            var storageConnectionString = Configuration["ConnectionStrings:StorageConnection"];
            // Get table storage connection string for serilog
            CloudStorageAccount logStorageAccount = CloudStorageAccount.Parse(storageConnectionString);

            // Configure the Serilog pipeline
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.AzureTableStorage(logStorageAccount)
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FabricServiceSettings>(Configuration.GetSection("FabricServiceSettings"));
            // Add framework services.
            services.AddMvc();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "My DEMO API", Version = "v1" });
            });
                       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            // Add Serilog to the logging pipeline
            loggerFactory.AddSerilog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");
            });
        }
    }
}

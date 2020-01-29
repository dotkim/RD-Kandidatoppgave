using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace WebApplication1
{
    public class Startup
    {
        protected IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var appHost = new AppHost("TestApp", typeof(AppHost).Assembly)
            {
                AppSettings = new NetCoreAppSettings(Configuration)
            };
            ConfigureServiceStackApp(appHost, app, env, loggerFactory);
        }

        protected virtual void ConfigureServiceStackApp(AppHostBase appHost,
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            app.UseServiceStack(appHost);
        }
    }
}

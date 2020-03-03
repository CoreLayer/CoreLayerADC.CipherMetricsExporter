using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Funq;
using ServiceStack;
using CoreLayerADC.CipherMetricsExporter.ServiceInterface;
using ServiceStack.Logging;
using Prometheus;
using ServiceStack.Api.OpenApi;

namespace CoreLayerADC.CipherMetricsExporter
{
    public class Startup : ModularStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public new void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMetricServer();

            app.UseServiceStack(new AppHost
            {
                AppSettings = new NetCoreAppSettings(Configuration)
            });
        }
    }

    public class AppHost : AppHostBase
    {
        public AppHost() : base("CoreLayerADC.CipherMetricsExporter", typeof(CipherMetricsServices).Assembly) { }

        // Configure your AppHost with the necessary configuration and dependencies your App needs
        public override void Configure(Container container)
        {
            SetConfig(new HostConfig
            {
                DefaultRedirectPath = "/swagger-ui",
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false),
                EnableFeatures = Feature.Json | Feature.Metadata
            });
            
            LogManager.LogFactory = new ConsoleLogFactory();
            Plugins.Add(new OpenApiFeature());
            Plugins.Add(new RequestLogsFeature(100)
            {
                RequiredRoles = null
            });
        }
    }
}

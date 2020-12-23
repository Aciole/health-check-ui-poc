using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace HealthCheck.UI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services
                .AddHealthChecksUI(setup =>
                {
                    setup.AddHealthCheckEndpoint("Microsservice.One", "http://localhost:5001/hc");
                    setup.AddHealthCheckEndpoint("Microsservice.Two", "http://localhost:5002/hc");
                })
                .AddInMemoryStorage();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(config =>
            {
                config.MapControllers();
                config.MapHealthChecksUI();

            });

            app.UseHealthChecksUI(setup =>
            {
                setup.UIPath = "/hc-ui";
            });


        }
    }
}

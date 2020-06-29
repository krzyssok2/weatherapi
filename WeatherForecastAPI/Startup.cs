using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherForecastAPI.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;

namespace WeatherForecastAPI
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
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.Map("/weatherforecast", Page1);

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private static void Page1(IApplicationBuilder app)
        {

            app.Use(async (context, next) =>
            {

                await context.Response.WriteAsync("<p>FastLinks</p>" +
                    "<a href='/api/cities'>Go api cities</a>" +
                    "<p></p>" +
                    "<a href='/api/weather'>Go to api weather</a>" +
                    "<p></p>" +
                    "<a href='/api/Users'>Go to Users</a>" +
                    "<p></p>" +
                    "<a href='/swagger'>Go to Swagger</a>" +
                    "<p style=\"color:#808000;\">============================================</p>"+
                    "<a href='/api/weather/city/vilnius/provider/METEO'>Vilnius test METEO</a>" +
                    "<p></p>" +
                    "<a href='/api/weather/city/vilnius/provider/OWM'>Vilnius test OWM</a>" +
                    "<p></p>" +
                    "<a href='/api/weather/city/vilnius/provider/BBC'>Vilnius test BBC</a>");
                await next();
            });
        }

    }
}

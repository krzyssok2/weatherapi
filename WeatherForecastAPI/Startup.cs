using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherForecastAPI.Models;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Reflection;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;
using WeatherForecastAPI.Worker;
using System.Collections.Generic;
using WeatherForecastAPI.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
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
            services.AddDbContext<WeatherContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyWeatherAPIDatabase")));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<WeatherContext>().AddDefaultTokenProviders();
            services.AddControllers()
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddDbContext<WeatherContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyWeatherApi")));

            var jwtSettings = new JwtSettings();
            Configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });
            //Add httpclinet for providers
            services.AddHttpClient("OWM", c =>
            {
                c.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/");
                //c.DefaultRequestHeaders.Add("");

            });
            services.AddHttpClient("METEO", c =>
            {
                c.BaseAddress = new Uri("https://api.meteo.lt/v1/");
                //c.DefaultRequestHeaders.Add("");

            });
            services.AddHttpClient("BBC", c =>
            {
                c.BaseAddress = new Uri("https://weather-broker-cdn.api.bbci.co.uk/en/");
                //c.DefaultRequestHeaders.Add("");

            });

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[0] }
                };
                x.AddSecurityDefinition(name: "Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {new OpenApiSecurityScheme{Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }}, new List<string>()}
                });
            });
            
            services.AddTransient<IHostedService, FetcherService>();

            //Add Fetcher services
            services.AddScoped<IFetcher,BBCFetcher>();
            services.AddScoped<IFetcher,MeteoFetcher>();
            services.AddScoped<IFetcher,OWMFetcher>();
            services.AddScoped<OWMActualFetcher>();

            //Get List of all Ifetcher Interfaces
            services.AddScoped<List<IFetcher>>();


            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
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

            //app.Map("/weatherforecast", Page1);

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        //private static void Page1(IApplicationBuilder app)
        //{

        //    app.Use(async (context, next) =>
        //    {

        //        await context.Response.WriteAsync("<p>FastLinks</p>" +
        //            "<a href='/api/cities'>Go api cities</a>" +
        //            "<p></p>" +
        //            "<a href='/api/weather'>Go to api weather</a>" +
        //            "<p></p>" +
        //            "<a href='/api/Users'>Go to Users</a>" +
        //            "<p></p>" +
        //            "<a href='/swagger'>Go to Swagger</a>" +
        //            "<p style=\"color:#808000;\">============================================</p>");
        //        await next();
        //    });
        //}

    }
}

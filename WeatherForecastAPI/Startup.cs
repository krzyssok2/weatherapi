using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;
using System.Collections.Generic;
using WeatherForecastAPI.Options;
using WeatherForecastAPI.Worker;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text;
using System.Linq;
using System.IO;
using System;
using WeatherForecastAPI.ErrorHandler;
using WeatherForecastAPI.Models;
using WeatherForecastAPI.Services;

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

            var jwtSettings = new JwtSettings();
            Configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            var tokenValidationParameters= new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => 
            {
                x.TokenValidationParameters = tokenValidationParameters;
            });



            //Add httpclinet for providers
            services.AddHttpClient("OWM", c =>
            {
                c.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/");
            });
            services.AddHttpClient("METEO", c =>
            {
                c.BaseAddress = new Uri("https://api.meteo.lt/v1/");
            });
            services.AddHttpClient("BBC", c =>
            {
                c.BaseAddress = new Uri("https://weather-broker-cdn.api.bbci.co.uk/en/");      
            });

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Weather API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[0] }
                };

                var scheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                x.AddSecurityDefinition(name: "Bearer", scheme);
     
                x.OperationFilter<SecurityRequirementsOperationFilter>(true, scheme.Reference.Id);

            });
            
            services.AddTransient<IHostedService, FetcherService>();

            //Add Fetcher services
            services.AddScoped<IFetcher,BBCFetcher>();
            services.AddScoped<IFetcher,MeteoFetcher>();
            services.AddScoped<IFetcher,OWMFetcher>();
            services.AddScoped<OWMActualFetcher>();

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options=>
                {
                    options.InvalidModelStateResponseFactory =
                    c => new BadRequestObjectResult(new ErrorResponse
                    {
                        Errors = c.ModelState.Select(errorDetails => new ErrorModel
                        {
                            FieldName = errorDetails.Key,
                            ErrorType = errorDetails.Value.Errors.Select(x => (EnumErrors)Enum.Parse(typeof(EnumErrors), x.ErrorMessage, true)).ToList()
                        }).ToList()
                    });
                })
                .AddFluentValidation(mvcConfiguration => mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });


            services.AddScoped<AuthServices>();
            services.AddScoped<ConverterService>();
            services.AddScoped<SettingsServices>();
            services.AddScoped<WeatherServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();        

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

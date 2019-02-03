using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Presence.Data;
using Presence.Data.Models;
using Presence.Infrastructure.Options;
using Presence.Services;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;

namespace Presence.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPresenceApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterCorsService(services);
            RegisterDbContext(services, configuration);
            RegisterOptions(services, configuration);
            RegisterIdentityUser(services, configuration);
            RegisterServices(services);
            RegisterSwagger(services);
            RegisterMvcServices(services);
        }

        private static void RegisterCorsService(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        private static void RegisterDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PresenceDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
            });
        }

        private static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        }

        private static void RegisterIdentityUser(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, UserRole>()
                .AddEntityFrameworkStores<PresenceDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!#$%&'*+-/=?^_`{|}~.@";

                // Password settings
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
            });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = configuration.GetSection(nameof(JwtSettings))[nameof(JwtSettings.Authority)],
                        ValidAudience = configuration.GetSection(nameof(JwtSettings))[nameof(JwtSettings.Audience)],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection(nameof(JwtSettings))[nameof(JwtSettings.Secret)])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }

        private static void RegisterSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = nameof(Api) });
            });
        }

        private static void RegisterMvcServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver =
                        new CamelCasePropertyNamesContractResolver();
                });
        }
    }
}

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
            services.RegisterCorsService();
            services.RegisterDbContext(configuration);
            services.RegisterOptions(configuration);
            services.RegisterIdentityUser(configuration);
            services.RegisterServices();
            services.RegisterSwagger();
            services.RegisterMvcServices();
        }

        private static void RegisterCorsService(this IServiceCollection services)
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

        private static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PresenceDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
            });
        }

        private static void RegisterOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        }

        private static void RegisterIdentityUser(this IServiceCollection services, IConfiguration configuration)
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
                        ValidIssuer = configuration.GetSection("JwtSettings")["Authority"],
                        ValidAudience = configuration.GetSection("JwtSettings")["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings")["Secret"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }

        private static void RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Presence API" });
            });
        }

        private static void RegisterMvcServices(this IServiceCollection services)
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

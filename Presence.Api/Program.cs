using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presence.Data;
using Presence.Data.Extensions;
using Presence.Data.Models;

namespace Presence.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var db = services.GetService<ApplicationDbContext>();
                var roleManager = services.GetService<RoleManager<UserRole>>();
                try
                {
                    db.SeedDatabase(roleManager).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    var logger = services.GetService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}

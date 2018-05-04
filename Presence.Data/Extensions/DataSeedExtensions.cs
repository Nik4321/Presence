using Microsoft.AspNetCore.Identity;
using Presence.Data.Models;
using Presence.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presence.Data.Extensions
{
    public static class DataSeedExtensions
    {
        private static string[] roles = new[]
        {
            RoleNamesConstants.Admin,
            RoleNamesConstants.Teacher,
            RoleNamesConstants.Student
        };

        private static async Task SeedRoles(this ApplicationDbContext db, RoleManager<UserRole> roleManager)
        {
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var newRole = new UserRole() { Name = role };
                    await roleManager.CreateAsync(newRole);
                }
            }
        }

        public static async Task SeedDatabase(this ApplicationDbContext db, RoleManager<UserRole> roleManager)
        {
            await db.SeedRoles(roleManager);
        }
    }
}

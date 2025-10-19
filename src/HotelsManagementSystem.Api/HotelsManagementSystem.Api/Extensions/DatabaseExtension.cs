using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

namespace HotelsManagementSystem.Api.Extensions
{
    public static class DatabaseExtension
    {
        public static async Task ApplyMigrationsAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

                if (pendingMigrations.Any())
                {
                    await dbContext.Database.MigrateAsync();
                }
            }
        }

        public static async Task SeedUsersAndRoles(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                var context = services.GetRequiredService<ApplicationDbContext>();

                // Seed roles
                await UsersAndRolesSeeder.SeedRolesAsync(roleManager);

                // Seed admin user
                await UsersAndRolesSeeder.SeedAdminUserAsync(userManager, context);

                // Seed receptionist user
                await UsersAndRolesSeeder.SeedReceptionistUserAsync(userManager, context);
            }
        }
    }
}

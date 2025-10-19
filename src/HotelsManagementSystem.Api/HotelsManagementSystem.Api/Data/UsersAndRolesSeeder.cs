using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelsManagementSystem.Api.Data
{
    public static class UsersAndRolesSeeder
    {
       public static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
       {
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.Admin));
            }

            if (!await roleManager.RoleExistsAsync(UserRoles.Receptionist))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.Receptionist));
            }

            if (!await roleManager.RoleExistsAsync(UserRoles.Customer))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.Customer));
            }
       }

        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            string username = "admin@example.com";
            string adminEmail = "admin@example.com";
            string adminPassword = "LBiz85Jn^WJbPz";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                ApplicationUser adminUser = new ApplicationUser
                {
                    UserName = username,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "Adminov",
                    JoinedOn = DateTime.UtcNow
                };

                IdentityResult result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);

                    var adminProfile = new Admin
                    {
                        UserId = adminUser.Id,
                        User = adminUser,
                    };

                    await context.Admins.AddAsync(adminProfile);    
                    await context.SaveChangesAsync();
                }
            }
        }

        public static async Task SeedReceptionistUserAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            string username = "receptionist@example.com";
            string receptionistEmail = "receptionist@example.com";
            string receptionistPassword = "GUSNyJgZM*pXo7";

            if (await userManager.FindByEmailAsync(receptionistEmail) == null)
            {
                ApplicationUser receptionistUser = new ApplicationUser
                {
                    UserName = username,
                    Email = receptionistEmail,
                    FirstName = "Receptionist",
                    LastName = "Receptionistov",
                    JoinedOn = DateTime.UtcNow
                };

                IdentityResult result = await userManager.CreateAsync(receptionistUser, receptionistPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(receptionistUser, UserRoles.Receptionist);

                    var receptionistProfile = new Receptionist
                    {
                        UserId = receptionistUser.Id,
                        User = receptionistUser,
                        HotelId = null, 
                        Hotel = null
                    };

                    await context.Receptionists.AddAsync(receptionistProfile);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

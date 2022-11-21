namespace AutoPlace.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Models;
    using Microsoft.AspNetCore.Identity;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ApplicationUsers.Any())
            {
                return;
            }

            var adminRole = dbContext.Roles.Where(x => x.Name == "Administrator").FirstOrDefault();
            var admin = new ApplicationUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                NormalizedEmail = "admin@admin.com".ToUpper(),
                NormalizedUserName = "admin@admin.com".ToUpper(),
            };

            admin.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(admin, "Test123!");

            admin.Roles.Add(new IdentityUserRole<string> { RoleId = adminRole.Id });

            var userRole = dbContext.Roles.Where(x => x.Name == "User").FirstOrDefault();
            var user = new ApplicationUser
            {
                UserName = "user@user.com",
                Email = "user@user.com",
                NormalizedEmail = "user@user.com".ToUpper(),
                NormalizedUserName = "user@user.com".ToUpper(),
            };
            user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, "Test123!");

            user.Roles.Add(new IdentityUserRole<string> { RoleId = userRole.Id });

            await dbContext.ApplicationUsers.AddAsync(admin);
            await dbContext.ApplicationUsers.AddAsync(user);
        }
    }
}

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

            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@admin.com",
            };

            var adminPassword = new PasswordHasher<ApplicationUser>().HashPassword(admin, "Test123!");
            admin.PasswordHash = adminPassword;

            var user = new ApplicationUser
            {
                UserName = "user",
                Email = "user@user.com",
            };

            var userPassword = new PasswordHasher<ApplicationUser>().HashPassword(user, "Test123!");
            user.PasswordHash = userPassword;

            await dbContext.ApplicationUsers.AddAsync(admin);
            await dbContext.ApplicationUsers.AddAsync(user);
        }
    }
}

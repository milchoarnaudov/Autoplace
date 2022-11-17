using Autoplace.Common;
using Autoplace.Common.Data.Services;
using Autoplace.Identity.Common;
using Autoplace.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Autoplace.Identity.Data
{
    public class DataSeeder : IDataSeeder
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DataSeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task SeedDataAsync()
        {
            await SeedRoleAsync();
            await SeedUser();
        }

        private async Task SeedRoleAsync()
        {
            if (await roleManager.RoleExistsAsync(SystemConstants.AdministratorRoleName))
            {
                return;
            }

            var adminRole = new IdentityRole(SystemConstants.AdministratorRoleName);
            var roleCreationResult = await roleManager.CreateAsync(adminRole);
            if (!roleCreationResult.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, roleCreationResult.Errors.Select(e => e.Description)));
            }
        }

        private async Task SeedUser()
        {
            if (userManager.Users.Any())
            {
                return;
            }

            var initialUser = new User
            {
                UserName = "apadmin",
                Email = "admin@autoplace.test",
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
            };

            var userCreationResult = await userManager.CreateAsync(initialUser);
            var token = await userManager.GeneratePasswordResetTokenAsync(initialUser);

            Console.WriteLine(token);

            if (!userCreationResult.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, userCreationResult.Errors.Select(e => e.Description)));
            }

            var assignRoleToUserResult = await userManager.AddToRoleAsync(initialUser, SystemConstants.AdministratorRoleName);
            if (!assignRoleToUserResult.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, assignRoleToUserResult.Errors.Select(e => e.Description)));
            }
        }
    }
}

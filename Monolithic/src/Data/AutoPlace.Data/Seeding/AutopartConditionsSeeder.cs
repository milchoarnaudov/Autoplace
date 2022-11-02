namespace AutoPlace.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Models;

    public class AutopartConditionsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.AutopartConditions.Any())
            {
                return;
            }

            await dbContext.AutopartConditions.AddAsync(new AutopartCondition { Name = "New" });
            await dbContext.AutopartConditions.AddAsync(new AutopartCondition { Name = "Used" });
        }
    }
}

namespace AutoPlace.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Models;

    public class AutopartCategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.AutopartCategories.Any())
            {
                return;
            }

            await dbContext.AutopartCategories.AddAsync(new AutopartCategory { Name = "Engine" });
            await dbContext.AutopartCategories.AddAsync(new AutopartCategory { Name = "Transmission" });
            await dbContext.AutopartCategories.AddAsync(new AutopartCategory { Name = "Steering" });
            await dbContext.AutopartCategories.AddAsync(new AutopartCategory { Name = "Body Parts" });
            await dbContext.AutopartCategories.AddAsync(new AutopartCategory { Name = "Exhaust System" });
            await dbContext.AutopartCategories.AddAsync(new AutopartCategory { Name = "Lights" });
            await dbContext.AutopartCategories.AddAsync(new AutopartCategory { Name = "Glass" });
        }
    }
}

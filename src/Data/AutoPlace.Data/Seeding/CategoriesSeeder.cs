namespace AutoPlace.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Models;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            await dbContext.Categories.AddAsync(new Category { Name = "Engine" });
            await dbContext.Categories.AddAsync(new Category { Name = "Transmission" });
            await dbContext.Categories.AddAsync(new Category { Name = "Steering" });
            await dbContext.Categories.AddAsync(new Category { Name = "Body Parts" });
            await dbContext.Categories.AddAsync(new Category { Name = "Exhaust System" });
            await dbContext.Categories.AddAsync(new Category { Name = "Lights" });
            await dbContext.Categories.AddAsync(new Category { Name = "Glass" });
        }
    }
}

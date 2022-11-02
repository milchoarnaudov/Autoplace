namespace AutoPlace.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Models;

    public class CarTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.CarTypes.Any())
            {
                return;
            }

            await dbContext.CarTypes.AddAsync(new CarType { Name = "Hatchback" });
            await dbContext.CarTypes.AddAsync(new CarType { Name = "SUV" });
            await dbContext.CarTypes.AddAsync(new CarType { Name = "Cabriolet" });
            await dbContext.CarTypes.AddAsync(new CarType { Name = "Sedan" });
        }
    }
}

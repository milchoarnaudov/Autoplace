namespace AutoPlace.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Models;

    public class CarModelsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.CarModels.Any())
            {
                return;
            }

            var audi = new CarManufacturer { Name = "Audi" };
            var bmw = new CarManufacturer { Name = "BMW" };
            var mercedes = new CarManufacturer { Name = "Mercedes" };

            for (int i = 3; i <= 8; i++)
            {
                await dbContext.CarModels.AddAsync(new CarModel { Name = $"A{i}", Manufacturer = audi, });
                await dbContext.CarModels.AddAsync(new CarModel { Name = $"{i} Series", Manufacturer = bmw, });
            }

            await dbContext.CarModels.AddAsync(new CarModel { Name = $"A-Class", Manufacturer = mercedes, });
            await dbContext.CarModels.AddAsync(new CarModel { Name = $"C-Class", Manufacturer = mercedes, });
            await dbContext.CarModels.AddAsync(new CarModel { Name = $"E-Class", Manufacturer = mercedes, });
            await dbContext.CarModels.AddAsync(new CarModel { Name = $"S-Class", Manufacturer = mercedes, });
        }
    }
}

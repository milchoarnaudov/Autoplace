using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Data.Services;

namespace Autoplace.Autoparts.Data
{
    public class DataSeeder : IDataSeeder
    {
        private readonly AutopartsDbContext autopartsDbContext;

        public DataSeeder(AutopartsDbContext autopartsDbContext)
        {
            this.autopartsDbContext = autopartsDbContext;
        }

        public async Task SeedDataAsync()
        {
            await SeedCarsAsync();
            await SeedAutopartCategoriesAsync();
            await SeedAutopartConditionsAsync();
            await SeedCarTypesAsync();

            await autopartsDbContext.SaveChangesAsync();
        }

        private async Task SeedAutopartCategoriesAsync()
        {
            if (autopartsDbContext.AutopartCategories.Any())
            {
                return;
            }

            await autopartsDbContext.AutopartCategories.AddAsync(new AutopartCategory { Name = "Engine" });
            await autopartsDbContext.AutopartCategories.AddAsync(new AutopartCategory { Name = "Transmission" });
            await autopartsDbContext.AutopartCategories.AddAsync(new AutopartCategory { Name = "Steering" });
            await autopartsDbContext.AutopartCategories.AddAsync(new AutopartCategory { Name = "Body Parts" });
            await autopartsDbContext.AutopartCategories.AddAsync(new AutopartCategory { Name = "Exhaust System" });
            await autopartsDbContext.AutopartCategories.AddAsync(new AutopartCategory { Name = "Lights" });
            await autopartsDbContext.AutopartCategories.AddAsync(new AutopartCategory { Name = "Glass" });
        }

        private async Task SeedAutopartConditionsAsync()
        {
            if (autopartsDbContext.AutopartConditions.Any())
            {
                return;
            }

            await autopartsDbContext.AutopartConditions.AddAsync(new AutopartCondition { Name = "New" });
            await autopartsDbContext.AutopartConditions.AddAsync(new AutopartCondition { Name = "Used" });
        }

        private async Task SeedCarTypesAsync()
        {
            if (autopartsDbContext.CarTypes.Any())
            {
                return;
            }

            await autopartsDbContext.CarTypes.AddAsync(new CarType { Name = "Hatchback" });
            await autopartsDbContext.CarTypes.AddAsync(new CarType { Name = "SUV" });
            await autopartsDbContext.CarTypes.AddAsync(new CarType { Name = "Cabriolet" });
            await autopartsDbContext.CarTypes.AddAsync(new CarType { Name = "Sedan" });
        }

        private async Task SeedCarsAsync()
        {
            if (autopartsDbContext.Cars.Any())
            {
                return;
            }

            var audi = new CarManufacturer { Name = "Audi" };
            var bmw = new CarManufacturer { Name = "BMW" };
            var mercedes = new CarManufacturer { Name = "Mercedes" };

            var hatchbackCarType = new CarType { Name = "Hatchback" };
            var suvCarType = new CarType { Name = "SUV" };
            var cabriolet = new CarType { Name = "Cabriolet" };
            var sedanCarType = new CarType { Name = "Sedan" };
            var wagonCarType = new CarType { Name = "Wagon" };

            for (int i = 1; i <= 8; i++)
            {
                if (i <= 3)
                {
                    await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = $"A{i}", Manufacturer = audi }, CarType = hatchbackCarType });

                    continue;
                }

                await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = $"A{i}", Manufacturer = audi }, CarType = sedanCarType });

                if (i == 6 || i == 4)
                {
                    await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = $"A{i}", Manufacturer = audi }, CarType = wagonCarType });
                }
            }

            await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = $"A-Class", Manufacturer = mercedes, }, CarType = hatchbackCarType });
            await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = $"C-Class", Manufacturer = mercedes, }, CarType = sedanCarType });
            await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = $"C-Class", Manufacturer = mercedes, }, CarType = wagonCarType });
            await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = $"E-Class", Manufacturer = mercedes, }, CarType = sedanCarType });
            await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = $"E-Class", Manufacturer = mercedes, }, CarType = wagonCarType });
            await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = $"S-Class", Manufacturer = mercedes, }, CarType = sedanCarType });

            await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = "X5", Manufacturer = bmw, }, CarType = suvCarType });
            await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = "X6", Manufacturer = bmw, }, CarType = suvCarType });

            for (int i = 3; i < 7; i++)
            {
                if (i == 3 || i == 5)
                {
                    if (i == 3)
                    {
                        await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = $"{i} Series", Manufacturer = bmw, }, CarType = cabriolet });
                    }

                    await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = $"{i} Series", Manufacturer = bmw, }, CarType = sedanCarType });
                    await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = $"{i} Series", Manufacturer = bmw, }, CarType = wagonCarType });

                    continue;
                }

                await autopartsDbContext.Cars.AddAsync(new Car { Model = new CarModel { Name = $"{i} Series", Manufacturer = bmw, }, CarType = sedanCarType });
            }

        }
    }
}

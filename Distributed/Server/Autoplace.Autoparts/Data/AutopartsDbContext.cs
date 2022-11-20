using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Autoplace.Autoparts.Data
{
    public class AutopartsDbContext : BaseDbContextWithMessaging
    {
        public AutopartsDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Autopart> Autoparts { get; set; }

        public DbSet<AutopartCondition> AutopartConditions { get; set; }

        public DbSet<Car> Cars { get; set; }

        public DbSet<CarManufacturer> CarManufacturers { get; set; }

        public DbSet<CarModel> CarModels { get; set; }

        public DbSet<CarType> CarTypes { get; set; }

        public DbSet<AutopartCategory> AutopartCategories { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();
    }
}

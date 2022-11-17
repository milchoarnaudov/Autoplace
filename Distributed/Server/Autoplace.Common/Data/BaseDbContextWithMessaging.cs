using System.Reflection;
using Autoplace.Common.Data;
using Autoplace.Common.Messaging;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Data
{

    public abstract class BaseDbContextWithMessaging : BaseDbContext
    {
        protected BaseDbContextWithMessaging(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }

        protected abstract Assembly ConfigurationsAssembly { get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new MessageConfiguration());

            builder.ApplyConfigurationsFromAssembly(this.ConfigurationsAssembly);

            base.OnModelCreating(builder);
        }
    }
}

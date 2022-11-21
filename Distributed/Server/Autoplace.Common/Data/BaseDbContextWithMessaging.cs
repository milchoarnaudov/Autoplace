using System.Reflection;
using Autoplace.Common.Data.Configurations;
using Autoplace.Common.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Autoplace.Common.Data
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
            builder.ApplyConfiguration(new MessageConfiguration());

            builder.ApplyConfigurationsFromAssembly(ConfigurationsAssembly);

            base.OnModelCreating(builder);
        }
    }
}

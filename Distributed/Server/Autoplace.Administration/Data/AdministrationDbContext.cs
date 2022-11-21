using Autoplace.Administration.Data.Models;
using Autoplace.Common.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Autoplace.Administration.Data
{
    public class AdministrationDbContext : BaseDbContextWithMessaging
    {
        public AdministrationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ApprovalRequest> ApprovalRequests { get; set; }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();
    }
}

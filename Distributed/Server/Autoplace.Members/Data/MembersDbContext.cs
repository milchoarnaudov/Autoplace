using Autoplace.Common.Data;
using Autoplace.Members.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Autoplace.Members.Data
{
    public class MembersDbContext : BaseDbContextWithMessaging
    {
        public MembersDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();
    }
}

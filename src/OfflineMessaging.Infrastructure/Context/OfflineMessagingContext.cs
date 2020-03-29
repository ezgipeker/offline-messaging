using Microsoft.EntityFrameworkCore;
using OfflineMessaging.Domain.Entities;
using System.Linq;

namespace OfflineMessaging.Infrastructure.Context
{
    public class OfflineMessagingContext : DbContext
    {
        public OfflineMessagingContext(DbContextOptions<OfflineMessagingContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}

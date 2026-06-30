using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class DatabaseContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}

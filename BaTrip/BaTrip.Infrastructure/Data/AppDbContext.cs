using BaTrip.Domain.Entities;
using BaTrip.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BaTrip.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() 
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new FavoriteConfiguration());
        }
    }
}

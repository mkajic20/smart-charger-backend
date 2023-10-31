
using Microsoft.EntityFrameworkCore;
using SmartCharger.Data.Entities;

namespace SmartCharger.Data
{
    public class SmartChargerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Charger> Chargers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Card> Cards { get; set; }
        public SmartChargerContext(DbContextOptions<SmartChargerContext> options) :  base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(
                new Role {Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Customer" });   
            }
        }
    }


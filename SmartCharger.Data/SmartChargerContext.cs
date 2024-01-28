
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
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Customer" });

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@gmail.com",
                    Password = "$2b$10$hmyMVezD9FUJdULq3sFnI.oWajZxHcuWusLmIdCUTcg5XI9Zy4R3a",
                    Active = true,
                    CreationTime = DateTime.UtcNow,
                    Salt = "$2b$10$f1wBguLF9ane/U9yySuKau",
                    RoleId = 1
                },
                new User
                {
                    Id = 2,
                    FirstName = "Customer",
                    LastName = "Customer",
                    Email = "customer@gmail.com",
                    Password = "$2b$10$Zeaai0Ju24WY/x.cpZpFiOCZAyjdphShUUqCIeoPpPEn.LPBDB1uy",
                    Active = true,
                    CreationTime = DateTime.UtcNow,
                    Salt = "$2b$10$Zeaai0Ju24WY/x.cpZpFiO",
                    RoleId = 2
                });
        }
    }
}
  


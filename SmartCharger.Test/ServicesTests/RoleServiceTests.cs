using Microsoft.EntityFrameworkCore;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Services;
using SmartCharger.Data.Entities;
using SmartCharger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SmartCharger.Test.ServicesTests
{
    public class RoleServiceTests
    {
        [Fact]
        public async Task GetAllRoles_WhenRolesExist_ShouldReturnListOfRoles()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "RoleServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var roleService = new RoleService(context);

                // Act
                RoleResponseDTO result = await roleService.GetAllRoles();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("List of roles.", result.Message);
                Assert.NotNull(result.Roles);
                Assert.Equal(1, result.Roles[0].Id);
                Assert.Equal("Admin", result.Roles[0].Name);
                Assert.Equal(2, result.Roles[1].Id);
                Assert.Equal("User", result.Roles[1].Name);
                Dispose(context);
            }
        }

        private void SetupDatabase(SmartChargerContext context)
        {
            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }

        private void Dispose(SmartChargerContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}


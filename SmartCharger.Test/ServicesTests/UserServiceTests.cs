using Microsoft.EntityFrameworkCore;
using Moq;
using SmartCharger.Business.Services;
using SmartCharger.Data.Entities;
using SmartCharger.Data;
using SmartCharger.Business.DTOs;

namespace SmartCharger.Test.ServicesTests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetAllUsers_WhenUsersExists_ShouldReturnListOfUsers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }
            using (var context = new SmartChargerContext(options))
            {
                var userService = new UserService(context);

                // Act
                UsersResponseDTO result = await userService.GetAllUsers();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("List of users.", result.Message);
                Assert.NotNull(result.Users);
                Assert.Equal("1", result.Users[0].Id.ToString());
                Assert.Equal("Ivo", result.Users[0].FirstName);
                Assert.Equal("Ivic", result.Users[0].LastName);
                Assert.Equal("iivic@example.com", result.Users[0].Email);
                Assert.Equal("2", result.Users[1].Id.ToString());
                Assert.Equal("Ivo", result.Users[1].FirstName);
                Assert.Equal("Ivic", result.Users[1].LastName);
                Assert.Equal("iivic2@example.com", result.Users[1].Email);
                Dispose(context);
            }

        }
        [Fact]
        public async Task GetAllUsers_WhenUsersDontExist_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "Empty")
                .Options;



            using (var context = new SmartChargerContext(options))
            {
                var userService = new UserService(context);

                // Act
                UsersResponseDTO result = await userService.GetAllUsers();

                // Assert
                Assert.False(result.Success);
                Assert.Equal("There are no users with that parameters.", result.Message);
                Assert.Null(result.Users);

            }
        }

        private void Dispose(SmartChargerContext context)
        {
            context.DisposeAsync();
        }

        [Fact]
        public async Task GetUserById_WhenUserExists_ShouldReturnUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            var userId = 1;
            using (var context = new SmartChargerContext(options))
            {
                var userService = new UserService(context);

                // Act
                SingleUserResponseDTO result = await userService.GetUserById(userId);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("User found.", result.Message);
                Assert.NotNull(result.User);
                Assert.Equal(userId, result.User.Id);
                Assert.Equal("Ivo", result.User.FirstName);
                Assert.Equal("Ivic", result.User.LastName);
                Assert.Equal("iivic@example.com", result.User.Email);
                Dispose(context);
            }
        }

        [Fact]
        public async Task GetUserById_WhenUserNotExists_ShouldReturnUserNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            var nonExistingUserId = 1000; 

            using (var context = new SmartChargerContext(options))
            {
                var userService = new UserService(context);

                // Act
                SingleUserResponseDTO result = await userService.GetUserById(nonExistingUserId);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("User not found.", result.Message);
                Assert.Null(result.User);
                Dispose(context);
            }
        }

        [Fact]
        public async Task UpdateActiveStatus_WhenUserFound_ShouldUpdateAndReturnSuccessResponse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            var userId = 1;
            using (var context = new SmartChargerContext(options))
            {
                var userService = new UserService(context);

                // Act
                SingleUserResponseDTO result = await userService.UpdateActiveStatus(userId);

                // Assert
                Assert.True(result.Success);
                Assert.NotNull(result.User);
                Assert.Equal(userId, result.User.Id);
                Assert.Equal("Ivo", result.User.FirstName);
                Assert.Equal("Ivic", result.User.LastName);
                Assert.Equal(false, result.User.Active);
                Assert.Equal($"User {result.User.FirstName} {result.User.LastName} is {((bool)result.User.Active ? "activated" : "deactivated")}.", result.Message);
                Dispose(context);
            }
        }

        [Fact]
        public async Task UpdateActiveStatus_WhenUserNotFound_ShouldReturnUserNotFoundResponse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            var nonExistingUserId = 1000;
            using (var context = new SmartChargerContext(options))
            {
                var userService = new UserService(context);

                // Act
                SingleUserResponseDTO result = await userService.UpdateActiveStatus(nonExistingUserId);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("User not found.", result.Message);
                Assert.Null(result.User);
                Dispose(context);
            }
        }
        [Fact]
        public async Task UpdateRole_WhenUserFound_ShouldUpdateRoleAndReturnSuccess()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            var userId = 1;
            var roleId = 1;
            using (var context = new SmartChargerContext(options))
            {
                var userService = new UserService(context);

                // Act
                SingleUserResponseDTO result = await userService.UpdateRole(userId, roleId);

                // Assert
                Assert.True(result.Success);
                Assert.NotNull(result.User);
                Assert.Equal(userId, result.User.Id);
                Assert.Equal("Ivo", result.User.FirstName);
                Assert.Equal("Ivic", result.User.LastName);
                Assert.NotEqual(2, result.User.RoleId); 
                Assert.Equal(1, result.User.RoleId); 
                Assert.Equal($"User {result.User.FirstName} {result.User.LastName}'s role has been updated.", result.Message);
                Dispose(context);
            }
        }
        [Fact]
        public async Task UpdateRole_WhenUserNotFound_ShouldReturnUserNotFoundResponse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            var nonExistingUserId = 1000;
            var roleId = 2;
            using (var context = new SmartChargerContext(options))
            {
                var userService = new UserService(context);

                // Act
                SingleUserResponseDTO result = await userService.UpdateRole(nonExistingUserId, roleId);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("User not found.", result.Message);
                Assert.Null(result.User);
                Dispose(context);
            }
        }

        [Fact]
        public async Task UpdateRole_WhenUserRoleIsAlreadySet_ShouldReturnNoChangesMade()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceDatabaseRole")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            var userId = 1;
            var roleId = 2;
            using (var context = new SmartChargerContext(options))
            {
                var userService = new UserService(context);

                // Act
                SingleUserResponseDTO result = await userService.UpdateRole(userId, roleId);

                // Assert
                Assert.False(result.Success);
                Assert.NotNull(result.User);
                Assert.Equal(userId, result.User.Id);
                Assert.Equal("Ivo", result.User.FirstName);
                Assert.Equal("Ivic", result.User.LastName);
                Assert.Equal(2, result.User.RoleId);
                Assert.Equal($"User {result.User.FirstName} {result.User.LastName}'s role is already set to that role. No changes made.", result.Message);
                Dispose(context);
            }
        }





        private void SetupDatabase(SmartChargerContext context)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123", salt);

            context.Users.Add(new User
            {
                FirstName = "Ivo",
                LastName = "Ivic",
                Email = "iivic@example.com",
                Password = hashedPassword,
                Active = true,
                CreationTime = DateTime.Now.ToUniversalTime(),
                Salt = salt,
                RoleId = 2
            });

            context.Users.Add(new User
            {
                FirstName = "Ivo",
                LastName = "Ivic",
                Email = "iivic2@example.com",
                Password = hashedPassword,
                Active = false,
                CreationTime = DateTime.Now.ToUniversalTime(),
                Salt = salt,
                RoleId = 2
            });

            context.SaveChanges();
        }
    }
}

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

namespace SmartCharger.Test.ServicesTests
{
    public class LoginServiceTests
    {
        [Fact]
        public async Task LoginAsync_WhenUserRegisteredAndValidCredentials_ShouldReturnSuccess()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "RegisterServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            var loginDTO = new LoginDTO
            {
                Email = "iivic@example.com",
                Password = "password123"
            };

            using (var context = new SmartChargerContext(options))
            {
                var loginService = new LoginService(context);

                // Act
                LoginResponseDTO result = await loginService.LoginAsync(loginDTO);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("Login successful.", result.Message);
                Assert.Null(result.Error);
                Assert.Equal("iivic@example.com", result.User.Email);
                Assert.NotNull(result.Token);
            }
        }

        [Fact]
        public async Task LoginAsync_WhenUserRegisteredAndInvalidCredentials_ShouldReturnError()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "RegisterServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            var loginDTO = new LoginDTO
            {
                Email = "iivic@example.com",
                Password = "password321"
            };

            using (var context = new SmartChargerContext(options))
            {
                var loginService = new LoginService(context);

                // Act
                LoginResponseDTO result = await loginService.LoginAsync(loginDTO);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("Login failed.", result.Message);
                Assert.NotNull(result.Error);
                Assert.Null(result.User);
                Assert.Null(result.Token);
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

            context.SaveChanges();
        }
    }
}

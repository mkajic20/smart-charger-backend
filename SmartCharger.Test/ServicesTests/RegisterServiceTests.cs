using Microsoft.EntityFrameworkCore;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Services;
using SmartCharger.Data;
using SmartCharger.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharger.Test.ServicesTests
{
    public class RegisterServiceTests
    {
        [Fact]
        public async Task RegisterAsync_WhenValidRegistration_ShouldReturnSuccess()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "RegisterServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var registerService = new RegisterService(context);

                var registerDTO = new RegisterDTO
                {
                    FirstName = "Ivo",
                    LastName = "Ivic",
                    Email = "iivic@example.com",
                    Password = "password123"
                };

                // Act
                RegisterResponseDTO result = await registerService.RegisterAsync(registerDTO);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("Registration successful.", result.Message);
                Assert.NotNull(result.User);
                Assert.Equal("iivic@example.com", result.User.Email);
            }
        }

        [Fact]
        public async Task RegisterAsync_WhenInvalidEmail_ShouldReturnError()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "RegisterServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var registerService = new RegisterService(context);

                var registerDto = new RegisterDTO
                {
                    FirstName = "Ivo",
                    LastName = "Ivic",
                    Email = "invalid-email",
                    Password = "password123"
                };

                // Act
                var result = await registerService.RegisterAsync(registerDto);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("Registration failed.", result.Message);
                Assert.Contains("Email is not valid.", result.Error);
            }
        }

        [Fact]
        public async Task RegisterAsync_WhenDuplicateEmail_ShouldReturnError()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "RegisterServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var registerService = new RegisterService(context);

                context.Users.Add(new User
                {
                    FirstName = "Ivo",
                    LastName = "Ivic",
                    Email = "existing@example.com",
                    Password = "hashedpassword",
                    Active = true,
                    CreationTime = DateTime.Now.ToUniversalTime(),
                    Salt = "salt",
                    RoleId = 2
                });
                context.SaveChanges();

                var registerDto = new RegisterDTO
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "existing@example.com",
                    Password = "password123"
                };

                // Act
                var result = await registerService.RegisterAsync(registerDto);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("Registration failed.", result.Message);
                Assert.Contains("Email is already in use.", result.Error);
            }
        }

        [Fact]
        public async Task RegisterAsync_WhenInvalidPassword_ShouldReturnError()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "RegisterServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var registerService = new RegisterService(context);

                var registerDto = new RegisterDTO
                {
                    FirstName = "Ivo",
                    LastName = "Ivic",
                    Email = "iivic@example.com",
                    Password = "pass"
                };

                // Act
                var result = await registerService.RegisterAsync(registerDto);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("Registration failed.", result.Message);
                Assert.Contains("Password must have at least 6 characters.", result.Error);
            }
        }
    }
}

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Services;
using SmartCharger.Data;
using SmartCharger.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartCharger.Test.ServicesTests
{
    public class ChargerServiceTests
    {
        [Fact]
        public async Task CreateNewCharger_WithValidData_ShouldReturnSuccessResponse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "ChargerServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var chargerService = new ChargerService(context);

                var chargerDTO = new ChargerDTO
                {
                    Name = "Test Charger",
                    Latitude = 40.7128,
                    Longitude = -74.0060,
                    CreatorId = 1
                };

                // Act
                ChargerResponseDTO result = await chargerService.CreateNewCharger(chargerDTO);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("Charger created successfully.", result.Message);
                Assert.NotNull(result.Charger);
                Assert.Equal(chargerDTO.Name, result.Charger.Name);
                Assert.Equal(chargerDTO.Latitude, result.Charger.Latitude);
                Assert.Equal(chargerDTO.Longitude, result.Charger.Longitude);
                Assert.False(result.Charger.Active);
                Assert.NotNull(result.Charger.CreationTime);
                Assert.Null(result.Error);
            }
        }

        [Fact]
        public async Task CreateNewCharger_WithInvalidLatitude_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "ChargerServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var chargerService = new ChargerService(context);

                var chargerDTO = new ChargerDTO
                {
                    Name = "Test Charger",
                    Latitude = 400.7128,
                    Longitude = -74.0060,
                    CreatorId = 1
                };

                // Act
                ChargerResponseDTO result = await chargerService.CreateNewCharger(chargerDTO);

                // Assert    
                Assert.False(result.Success);
                Assert.Equal("Charger creation failed.", result.Message);
                Assert.Equal("Latitude must be between -90 and 90.", result.Error);
                Assert.Null(result.Charger);
                Assert.NotNull(result.Error);
            }
        }

        [Fact]
        public async Task CreateNewCharger_WithInvalidLongitude_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "ChargerServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var chargerService = new ChargerService(context);

                var chargerDTO = new ChargerDTO
                {
                    Name = "Test Charger",
                    Latitude = 40.7128,
                    Longitude = -740.0060,
                    CreatorId = 1
                };

                // Act
                ChargerResponseDTO result = await chargerService.CreateNewCharger(chargerDTO);

                // Assert    
                Assert.False(result.Success);
                Assert.Equal("Charger creation failed.", result.Message);
                Assert.Equal("Longitude must be between -180 and 180.", result.Error);
                Assert.Null(result.Charger);
                Assert.NotNull(result.Error);
            }
        }

        public async Task CreateNewCharger_WithInvalidName_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "ChargerServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var chargerService = new ChargerService(context);

                var chargerDTO = new ChargerDTO
                {
                    Name = "",
                    Latitude = 40.7128,
                    Longitude = -74.0060,
                    CreatorId = 1
                };

                // Act
                ChargerResponseDTO result = await chargerService.CreateNewCharger(chargerDTO);

                // Assert    
                Assert.False(result.Success);
                Assert.Equal("Charger creation failed.", result.Message);
                Assert.Equal("Name of the charger cannot be empty.", result.Error);
                Assert.Null(result.Charger);
                Assert.NotNull(result.Error);
            }
        }


        [Fact]
        public async Task GetAllChargers_WhenChargersExist_ShouldReturnListOfChargers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "ChargerServiceDatabaseGet")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupChargerDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var chargerService = new ChargerService(context);

                // Act
                ChargerResponseDTO result = await chargerService.GetAllChargers();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("List of chargers.", result.Message);
                Assert.NotNull(result.Chargers);
                Assert.Equal(2, result.Chargers.Count);
                Assert.Equal("Charger 2", result.Chargers[0].Name);
                Assert.Equal(2, result.Chargers[0].Id);
                Assert.Equal("Charger 1", result.Chargers[1].Name);
                Assert.Equal(1, result.Chargers[1].Id);
            }
        }


        [Fact]
        public async Task GetAllChargers_WhenChargersDontExist_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "Empty")
                .Options;



            using (var context = new SmartChargerContext(options))
            {
                var chargerService = new ChargerService(context);

                // Act
                ChargerResponseDTO result = await chargerService.GetAllChargers();

                // Assert
                Assert.False(result.Success);
                Assert.Equal("There are no chargers with that parameters.", result.Message);
                Assert.Null(result.Chargers);

            }
        }


        [Fact]
        public async Task UpdateCharger_WhenChargerValid_ShouldReturnSuccess()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "ChargerServiceDatabaseUpdate1")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupChargerDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var chargerService = new ChargerService(context);

                // Act
                ChargerResponseDTO result = await chargerService.GetAllChargers();
                var firstCharger = result.Chargers[0];

                var updatedCharger = new ChargerDTO
                {
                    Id = firstCharger.Id,
                    Name = "Updated charger",
                    Latitude = 50,
                    Longitude = 60
                };

                var updatedResult = await chargerService.UpdateCharger((int)firstCharger.Id, updatedCharger);


                // Assert
                Assert.True(updatedResult.Success);
                Assert.NotEqual(firstCharger.Name, updatedResult.Charger.Name);
                Assert.NotEqual(firstCharger.Latitude, updatedResult.Charger.Latitude);
                Assert.NotEqual(firstCharger.Longitude, updatedResult.Charger.Longitude);
                Assert.Equal("Charger updated successfully.", updatedResult.Message);
                Assert.Equal("Updated charger", updatedResult.Charger.Name);
                Assert.Equal(50, updatedResult.Charger.Latitude);
                Assert.Equal(60, updatedResult.Charger.Longitude);

            }
        }


        [Fact]
        public async Task UpdateCharger_WhenNameEmpty_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "ChargerServiceDatabaseUpdate2")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupChargerDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var chargerService = new ChargerService(context);

                // Act
                ChargerResponseDTO result = await chargerService.GetAllChargers();
                var firstCharger = result.Chargers[0];

                var updatedCharger = new ChargerDTO
                {
                    Id = firstCharger.Id,
                    Name = "",
                    Latitude = 50,
                    Longitude = 60
                };

                var updatedResult = await chargerService.UpdateCharger((int)firstCharger.Id, updatedCharger);


                // Assert
                Assert.False(updatedResult.Success);
                Assert.Equal("Charger update failed.", updatedResult.Message);
                Assert.Equal("Name of the charger cannot be empty.", updatedResult.Error);

            }
        }

        [Fact]
        public async Task UpdateCharger_WhenInvalidLatitude_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "ChargerServiceDatabaseUpdate3")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupChargerDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var chargerService = new ChargerService(context);

                // Act
                ChargerResponseDTO result = await chargerService.GetAllChargers();
                var firstCharger = result.Chargers[0];

                var updatedCharger = new ChargerDTO
                {
                    Id = firstCharger.Id,
                    Name = "Updated charger",
                    Latitude = -500,
                    Longitude = 60
                };

                var updatedResult = await chargerService.UpdateCharger((int)firstCharger.Id, updatedCharger);


                // Assert
                Assert.False(updatedResult.Success);
                Assert.Equal("Charger update failed.", updatedResult.Message);
                Assert.Equal("Latitude must be between -90 and 90.", updatedResult.Error);

            }
        }

        [Fact]
        public async Task UpdateCharger_WhenInvalidLongitude_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "ChargerServiceDatabaseUpdate4")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupChargerDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var chargerService = new ChargerService(context);

                // Act
                ChargerResponseDTO result = await chargerService.GetAllChargers();
                var firstCharger = result.Chargers[0];

                var updatedCharger = new ChargerDTO
                {
                    Id = firstCharger.Id,
                    Name = "Updated charger",
                    Latitude = -50,
                    Longitude = -600
                };

                var updatedResult = await chargerService.UpdateCharger((int)firstCharger.Id, updatedCharger);


                // Assert
                Assert.False(updatedResult.Success);
                Assert.Equal("Charger update failed.", updatedResult.Message);
                Assert.Equal("Longitude must be between -180 and 180.", updatedResult.Error);

            }
        }

        [Fact]
        public async Task DeleteCharger_WhenChargerExists_ShouldReturnSuccess()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "ChargerServiceDatabase5")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupChargerDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var chargerService = new ChargerService(context);

                // Act
                ChargerResponseDTO result = await chargerService.DeleteCharger(1);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("Charger deleted successfully.", result.Message);
                Assert.Null(result.Charger);
            }
        }


        [Fact]
        public async Task DeleteCharger_WhenChargerDoesntExist_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "Empty")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var chargerService = new ChargerService(context);

                // Act
                ChargerResponseDTO result = await chargerService.DeleteCharger(1);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("Unsuccessful deletion of the charger.", result.Message);
                Assert.Equal("Charger not found.", result.Error);
                Assert.Null(result.Charger);
            }

        }

        [Fact]
        public async Task GetChargerById_WhenChargerExists_ShouldReturnCharger()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "ChargerServiceDatabaseGetById1")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupChargerDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var chargerService = new ChargerService(context);

                // Act
                ChargerResponseDTO result = await chargerService.GetChargerById(1);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("Charger exists.", result.Message);
                Assert.NotNull(result.Charger);
                Assert.Equal("Charger 1", result.Charger.Name);
                Assert.Equal(1, result.Charger.Id);
                Assert.Equal("Charger 1", result.Charger.Name);
                Assert.Equal(40.7128, result.Charger.Latitude);
                Assert.Equal(-74.0060, result.Charger.Longitude);
            }
        }

        [Fact]
        public async Task GetChargerById_WhenChargerDoesNotExist_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "ChargerServiceDatabaseGetById")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupChargerDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var chargerService = new ChargerService(context);

                // Act
                ChargerResponseDTO result = await chargerService.GetChargerById(3);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("Charger with that ID doesn't exist.", result.Message);
                Assert.Null(result.Charger);
            }
        }


        private void SetupChargerDatabase(SmartChargerContext context)
        {
            var chargers = new List<Charger>
            {
                new Charger
                {
                    Id = 1,
                    Name = "Charger 1",
                    Latitude = 40.7128,
                    Longitude = -74.0060,
                    Active = true,
                    CreationTime = DateTime.Now.ToUniversalTime(),
                    CreatorId = 1
                },
                new Charger
                {
                     Id = 2,
                    Name = "Charger 2",
                    Latitude = 34.0522,
                    Longitude = -118.2437,
                    Active = true,
                    CreationTime = DateTime.Now.ToUniversalTime(),
                    CreatorId = 2
                }
            };

            context.Chargers.AddRange(chargers);
            context.SaveChanges();
        }
    }
}


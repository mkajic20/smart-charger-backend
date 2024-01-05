

using Microsoft.EntityFrameworkCore;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Services;
using SmartCharger.Data;
using SmartCharger.Data.Entities;

namespace SmartCharger.Test.ServicesTests
{
    public class EventServiceTests
    {

        [Fact]

        public async Task GetUsersCharhingHistory_WhenUsersExists_ShouldReturnListOfEvents()
        {

            //Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
               .UseInMemoryDatabase(databaseName: "EventServiceDatabase")
               .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var eventService = new EventService(context);


                //Act
                EventResponseDTO result = await eventService.GetUsersChargingHistory(1);

                //Assert
                Assert.True(result.Success);
                Assert.Equal("List of Ivo Ivic's events.", result.Message);
                Assert.Equal(result.Events[0].Id, 1);
                Assert.Equal(result.Events[0].Volume, 123);
                Assert.Equal("Ivo", result.Events[0].User.FirstName);
                Assert.Equal("Ivic", result.Events[0].User.LastName);
                Assert.Equal("iivic@example.com", result.Events[0].User.Email);
                Assert.Equal("Card 1", result.Events[0].Card.Name);
                Assert.Equal("RFID-ST34-56UV-7890", result.Events[0].Card.Value);
                Assert.Equal("New charger", result.Events[0].Charger.Name);
                Assert.Equal(50, result.Events[0].Charger.Latitude);
                Assert.Equal(40, result.Events[0].Charger.Longitude);
            }
        }

        [Fact]
        public async Task GetUsersCharhingHistory_WhenUsersDoesntExist_ShouldReturnFailure()
        {

            //Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
               .UseInMemoryDatabase(databaseName: "EventServiceDatabaseFail")
               .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var eventService = new EventService(context);


                //Act
                EventResponseDTO result = await eventService.GetUsersChargingHistory(2);

                //Assert
                Assert.False(result.Success);
                Assert.Equal("User not found.", result.Message);
                Assert.Null(result.Events);
            }
        }

        [Fact]
        public async Task GetUsersCharhingHistory_WhenSearchParametersAreNotValid_ShouldReturnFailure()
        {

            //Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
               .UseInMemoryDatabase(databaseName: "EventServiceDatabaseFail1")
               .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var eventService = new EventService(context);


                //Act
                EventResponseDTO result = await eventService.GetUsersChargingHistory(1, 1,5, "incorrect");

                //Assert
                Assert.False(result.Success);
                Assert.Equal("There are no events with that parameters.", result.Message);
                Assert.Null(result.Events);

            }
        }

        [Fact]
        public async Task StartCharging_WhenEverythingIsOk_ShouldReturnSuccess()
        {

            //Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
               .UseInMemoryDatabase(databaseName: "EventServiceDatabaseStart")
               .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var eventService = new EventService(context);

                EventResponseDTO result = await eventService.StartCharging(new EventChargingDTO
                {
                    StartTime = DateTime.Now.ToUniversalTime(),
                    ChargerId = 1,
                    CardId = 1,
                    UserId = 1
                });

                Assert.True(result.Success);
                Assert.Equal("Charging started.", result.Message);
                Assert.Equal(1, result.Event.ChargerId);
                Assert.Equal(1, result.Event.CardId);
                Assert.Equal(1, result.Event.UserId);
            }
        }

        [Fact]
        public async Task StartCharging_WhenUserIsNotActive_ShouldReturnFailure()
        {

            //Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
               .UseInMemoryDatabase(databaseName: "EventServiceDatabaseStartFail1")
               .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var eventService = new EventService(context);

                EventResponseDTO result = await eventService.StartCharging(new EventChargingDTO
                {
                    StartTime = DateTime.Now.ToUniversalTime(),
                    ChargerId = 1,
                    CardId = 2,
                    UserId = 3
                });

                Assert.False(result.Success);
                Assert.Equal("User is not active.", result.Message);
                Assert.Null(result.Event);
            }
        }

        [Fact]
        public async Task StartCharging_WhenCardIsNotActive_ShouldReturnFailure()
        {

            //Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
               .UseInMemoryDatabase(databaseName: "EventServiceDatabaseStartFail2")
               .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var eventService = new EventService(context);

                EventResponseDTO result = await eventService.StartCharging(new EventChargingDTO
                {
                    StartTime = DateTime.Now.ToUniversalTime(),
                    ChargerId = 1,
                    CardId = 3,
                    UserId = 3
                });

                Assert.False(result.Success);
                Assert.Equal("RFID card is not active.", result.Message);
                Assert.Null(result.Event);
            }
        }

        [Fact]
        public async Task StartCharging_WhenCardIsAlreadyInUse_ShouldReturnFailure()
        {

            //Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
               .UseInMemoryDatabase(databaseName: "EventServiceDatabaseStartFail3")
               .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var eventService = new EventService(context);

                EventResponseDTO result = await eventService.StartCharging(new EventChargingDTO
                {
                    StartTime = DateTime.Now.ToUniversalTime(),
                    ChargerId = 1,
                    CardId = 4,
                    UserId = 3
                });

                Assert.False(result.Success);
                Assert.Equal("RFID card is already in use.", result.Message);
                Assert.Null(result.Event);
            }
        }

        [Fact]
        public async Task StartCharging_WhenChargerIsAlreadyInUse_ShouldReturnFailure()
        {

            //Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
               .UseInMemoryDatabase(databaseName: "EventServiceDatabaseStartFail4")
               .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var eventService = new EventService(context);

                EventResponseDTO result = await eventService.StartCharging(new EventChargingDTO
                {
                    StartTime = DateTime.Now.ToUniversalTime(),
                    ChargerId = 3,
                    CardId = 1,
                    UserId = 1
                });

                Assert.False(result.Success);
                Assert.Equal("Charger is already in use.", result.Message);
                Assert.Null(result.Event);
            }
        }

        [Fact]
        public async Task EndCharging_WhenEverythingIsOk_ShouldReturnSuccess()
        {

            //Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
               .UseInMemoryDatabase(databaseName: "EventServiceDatabaseEnd")
               .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {

                var eventService = new EventService(context);

                EventResponseDTO result = await eventService.EndCharging(new EventChargingDTO
                {
                    Id = 2,
                    StartTime = DateTime.Now.ToUniversalTime(),
                    EndTime = DateTime.Now.ToUniversalTime(),
                    Volume = 12,
                    ChargerId = 3,
                    CardId = 1,
                    UserId = 1
                });

                Assert.True(result.Success);
                Assert.Equal("Charging has ended.", result.Message);
                Assert.Equal(2, result.Event.Id);
                Assert.Equal(12, result.Event.Volume);
                Assert.Equal(3, result.Event.ChargerId);
                Assert.Equal(1, result.Event.CardId);
                Assert.Equal(1, result.Event.UserId);
            }
        }

        [Fact]
        public async Task EndCharging_WhenChargingAlreadyEnded_ShouldReturnFailure()
        {

            //Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
               .UseInMemoryDatabase(databaseName: "EventServiceDatabaseEndFail1")
               .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {

                var eventService = new EventService(context);

                EventResponseDTO result = await eventService.EndCharging(new EventChargingDTO
                {
                    Id = 1,
                    StartTime = DateTime.Now.ToUniversalTime(),
                    EndTime = DateTime.Now.ToUniversalTime(),
                    Volume = 123,
                    ChargerId = 1,
                    CardId = 1,
                    UserId = 1
                });

                Assert.False(result.Success);
                Assert.Equal("Charging has already ended.", result.Message);
                Assert.Null(result.Event);
            }
        }


        [Fact]
        public async Task GetFullChargingHistory_WhenEventsExist_ShouldReturnListOfEvents()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "EventServiceDatabaseGetFullHistory")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var eventService = new EventService(context);

                // Act
                EventResponseDTO result = await eventService.GetFullChargingHistory();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("List of all events.", result.Message);
                Assert.NotNull(result.Events);
                Assert.Equal(1, result.Events.Count);
            }
        }
        [Fact]
        public async Task GetFullChargingHistory_WhenSearchParameterIsInvalid_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "EventServiceDatabaseGetFullHistorySearch")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var eventService = new EventService(context);

                // Act
                EventResponseDTO result = await eventService.GetFullChargingHistory(1, 5, "Mrkic");

                // Assert
                Assert.False(result.Success);
                Assert.Equal("There are no events with that parameters.", result.Message);
                Assert.Null(result.Events);
            }
        }

        private void SetupDatabase(SmartChargerContext context)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123", salt);

            context.Users.Add(new User
            {
                Id = 1,
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
                Id = 3,
                FirstName = "Pero",
                LastName = "Peric",
                Email = "pperic@example.com",
                Password = hashedPassword,
                Active = false,
                CreationTime = DateTime.Now.ToUniversalTime(),
                Salt = salt,
                RoleId = 2
            });

            context.Chargers.Add(new Charger
            {
                Id = 1,
                Name = "New charger",
                Latitude = 50,
                Longitude = 40,
                CreationTime = DateTime.Now.ToUniversalTime(),
                Active = false,
                CreatorId = 1
            });

            context.Chargers.Add(new Charger
            {
                Id = 3,
                Name = "Charger 3",
                Latitude = 40,
                Longitude = 50,
                CreationTime = DateTime.Now.ToUniversalTime(),
                Active = true,
                CreatorId = 1
            });


            context.Cards.Add(new Card
            {
                Id = 1,
                Value = "RFID-ST34-56UV-7890",
                Active = true,
                UsageStatus = false,
                Name = "Card 1",
                UserId = 1
            });


            context.Cards.Add(new Card
            {
                Id = 2,
                Value = "RFID-AF32-32DA-3219",
                Active = true,
                UsageStatus = false,
                Name = "Card 2",
                UserId = 3
            });
            context.Cards.Add(new Card
            {
                Id = 3,
                Value = "RFID-FJ32-42DA-4812",
                Active = false,
                UsageStatus = false,
                Name = "Card 3",
                UserId = 3
            });
            context.Cards.Add(new Card
            {
                Id = 4,
                Value = "RFID-FJ32-42DA-4812",
                Active = true,
                UsageStatus = true,
                Name = "Card 4",
                UserId = 3
            });


            context.Events.Add(new Event
            {
                Id = 1,
                StartTime = DateTime.Now.ToUniversalTime(),
                EndTime = DateTime.Now.ToUniversalTime(),
                Volume = 123,
                UserId =1,
                ChargerId = 1,
                CardId = 1
            });

            context.Events.Add(new Event
            {
                Id = 2,
                StartTime = DateTime.Now.ToUniversalTime(),
                UserId = 1,
                ChargerId = 3,
                CardId = 1
            });

            context.SaveChanges();
        }
    }
}

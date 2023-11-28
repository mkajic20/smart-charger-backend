

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



            context.Chargers.Add(new Charger
            {
                Id = 1,
                Name = "New charger",
                Latitude = 50,
                Longitude = 40,
                CreationTime = DateTime.Now.ToUniversalTime(),
                Active = true,
                CreatorId = 1
            });


            context.Cards.Add(new Card
            {
                Value = "RFID-ST34-56UV-7890",
                Active = true,
                Name = "Card 1",
                UserId = 1
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

            context.SaveChanges();
        }
    }
}

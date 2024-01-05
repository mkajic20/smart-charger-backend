

using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Controllers;

namespace SmartCharger.Test.ControllerTests
{
    public class EventControllerTests
    {
        [Fact]

        public async Task GetUsersChargingHistory_WhenEventServiceReturnsSuccess_ShouldReturnOk()
        {
            //Arrange
            var eventServiceMock = new Mock<IEventService>();
            eventServiceMock.Setup(service => service.GetUsersChargingHistory(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(
                new EventResponseDTO
                {
                    Success = true,
                    Message = "List of Ivo Ivic's events.",
                    Events = new List<EventDTO>
                    {
                        new EventDTO
                        {
                            Id = 1,
                            StartTime = DateTime.Now.ToUniversalTime(),
                            EndTime = DateTime.Now.ToUniversalTime(),
                            Volume = 123,
                            User = new UserDTO
                            {
                                Id = 1,
                                FirstName = "Ivo",
                                LastName = "Ivic",
                                Email = "iivic@example.com"
                            },
                            Charger = new ChargerDTO
                            {
                                 Id = 1,
                                 Name = "New charger",
                                 Latitude = 50,
                                 Longitude = 40,
                                 CreationTime = DateTime.Now.ToUniversalTime(),
                                 Active = true,
                                 CreatorId = 1
                            },
                            Card = new CardDTO
                            {
                                Value = "RFID-ST34-56UV-7890",
                                Active = true,
                                Name = "Card 1",
                                User = new UserDTO {
                                        Id = 1,
                                        FirstName = "Ivo",
                                        LastName = "Ivic",
                                        Email = "iivic@example.com"
                                }
                            }
                        }
                    }

                });

            var controller = new EventController(eventServiceMock.Object);

            //Act
            var actionResult = await controller.GetUsersChargingHistory(1);

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as EventResponseDTO;
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("List of Ivo Ivic's events.", response.Message);
            Assert.Equal(response.Events[0].Id, 1);
            Assert.Equal(response.Events[0].Volume, 123);
            Assert.Equal("Ivo", response.Events[0].User.FirstName);
            Assert.Equal("Ivic", response.Events[0].User.LastName);
            Assert.Equal("iivic@example.com", response.Events[0].User.Email);
            Assert.Equal("Card 1", response.Events[0].Card.Name);
            Assert.Equal("RFID-ST34-56UV-7890", response.Events[0].Card.Value);
            Assert.Equal("New charger", response.Events[0].Charger.Name);
            Assert.Equal(50, response.Events[0].Charger.Latitude);
            Assert.Equal(40, response.Events[0].Charger.Longitude);

        }

        [Fact]
        public async Task GetUsersChargingHistory_WhenEventsUserNotExisting_ShouldReturnBadRequest()
        {
            //Arrange
            var eventServiceMock = new Mock<IEventService>();
            eventServiceMock.Setup(service => service.GetUsersChargingHistory(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(
                new EventResponseDTO
                {
                    Success = false,
                    Message = "User not found.",
                    Error = "User not found.",
                    Events = null
                });

            var controller = new EventController(eventServiceMock.Object);

            //Act
            var actionResult = await controller.GetUsersChargingHistory(1);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as EventResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("User not found.", response.Message);
            Assert.Equal("User not found.", response.Error);
            Assert.Null(response.Events);
        }

        [Fact]
        public async Task GetUsersChargingHistory_WhenSearchParameterInvalid_ShouldReturnBadRequest()
        {
            //Arrange
            var eventServiceMock = new Mock<IEventService>();
            eventServiceMock.Setup(service => service.GetUsersChargingHistory(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(
                new EventResponseDTO
                {
                    Success = false,
                    Message = "There are no events with that parameters.",
                    Events = null
                });

            var controller = new EventController(eventServiceMock.Object);

            //Act
            var actionResult = await controller.GetUsersChargingHistory(1);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as EventResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("There are no events with that parameters.", response.Message);
            Assert.Null(response.Events);
        }

        [Fact]
        public async Task StartCharging_WhenEventServiceReturnsSuccess_ShouldReturnOk()
        {
            //Arrange
            var eventServiceMock = new Mock<IEventService>();
            eventServiceMock.Setup(service => service.StartCharging(It.IsAny<EventChargingDTO>())).ReturnsAsync(
                new EventResponseDTO
                {
                    Success = true,
                    Message = "Charging started.",
                    Event = new EventChargingDTO
                    {
                        Id = 1,
                        StartTime = DateTime.Now.ToUniversalTime(),
                        ChargerId = 1,
                        CardId = 1,
                        UserId = 1
                    }
                });

            var controller = new EventController(eventServiceMock.Object);

            //Act
            var actionResult = await controller.StartCharging(new EventChargingDTO
            {
                StartTime = DateTime.Now.ToUniversalTime(),
                ChargerId = 1,
                CardId = 1,
                UserId = 1
            });

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as EventResponseDTO;
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("Charging started.", response.Message);
            Assert.Equal(response.Event.Id, 1);
        }

        [Fact]
        public async Task StartCharging_WhenEventServiceReturnsFailure_ShouldReturnBadRequest()
        {
            //Arrange
            var eventServiceMock = new Mock<IEventService>();
            eventServiceMock.Setup(service => service.StartCharging(It.IsAny<EventChargingDTO>())).ReturnsAsync(
                new EventResponseDTO
                {
                    Success = false,
                    Message = "Charger is already in use.",
                    Event = null
                });

            var controller = new EventController(eventServiceMock.Object);

            //Act
            var actionResult = await controller.StartCharging(new EventChargingDTO
            {
                StartTime = DateTime.Now.ToUniversalTime(),
                ChargerId = 1,
                CardId = 1,
                UserId = 1
            });

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as EventResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("Charger is already in use.", response.Message);
            Assert.Null(response.Event);
        }

        [Fact]
        public async Task EndCharging_WhenEventServiceReturnsSuccess_ShouldReturnOk()
        {
            //Arrange
            var eventServiceMock = new Mock<IEventService>();
            eventServiceMock.Setup(service => service.EndCharging(It.IsAny<EventChargingDTO>())).ReturnsAsync(
                new EventResponseDTO
                {
                    Success = true,
                    Message = "Charging has ended.",
                    Event = new EventChargingDTO
                    {
                        Id = 1,
                        StartTime = DateTime.Now.ToUniversalTime(),
                        EndTime = DateTime.Now.ToUniversalTime(),
                        ChargerId = 1,
                        CardId = 1,
                        UserId = 1
                    }
                });

            var controller = new EventController(eventServiceMock.Object);

            //Act
            var actionResult = await controller.EndCharging(new EventChargingDTO
            {
                StartTime = DateTime.Now.ToUniversalTime(),
                EndTime = DateTime.Now.ToUniversalTime(),
                ChargerId = 1,
                CardId = 1,
                UserId = 1
            });

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as EventResponseDTO;
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("Charging has ended.", response.Message);
            Assert.Equal(response.Event.Id, 1);
        }

        [Fact]
        public async Task EndCharging_WhenEventServiceReturnsFailure_ShouldReturnBadRequest()
        {
            //Arrange
            var eventServiceMock = new Mock<IEventService>();
            eventServiceMock.Setup(service => service.EndCharging(It.IsAny<EventChargingDTO>())).ReturnsAsync(
                new EventResponseDTO
                {
                    Success = false,
                    Message = "Charging has already ended.",
                    Event = null
                });

            var controller = new EventController(eventServiceMock.Object);

            //Act
            var actionResult = await controller.EndCharging(new EventChargingDTO
            {
                StartTime = DateTime.Now.ToUniversalTime(),
                EndTime = DateTime.Now.ToUniversalTime(),
                ChargerId = 1,
                CardId = 1,
                UserId = 1
            });

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as EventResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("Charging has already ended.", response.Message);
            Assert.Null(response.Event);
        }

        [Fact]
        public async Task GetFullChargingHistory_WhenEventServiceReturnsSuccess_ShouldReturnOk()
        {
            // Arrange
            var eventServiceMock = new Mock<IEventService>();
            eventServiceMock.Setup(service => service.GetFullChargingHistory(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(
                new EventResponseDTO
                {
                    Success = true,
                    Message = "List of all events.",
                    Events = new List<EventDTO>
                    {
                new EventDTO
                {
                    Id = 1,
                    StartTime = DateTime.Now.ToUniversalTime(),
                    EndTime = DateTime.Now.ToUniversalTime(),
                    Volume = 123,
                    User = new UserDTO
                    {
                        Id = 1,
                        FirstName = "Ivo",
                        LastName = "Ivic",
                        Email = "iivic@example.com"
                    },
                    Charger = new ChargerDTO
                    {
                        Id = 1,
                        Name = "New charger",
                        Latitude = 50,
                        Longitude = 40,
                        CreationTime = DateTime.Now.ToUniversalTime(),
                        Active = true,
                        CreatorId = 1
                    },
                    Card = new CardDTO
                    {
                        Value = "RFID-ST34-56UV-7890",
                        Active = true,
                        Name = "Card 1",
                        User = new UserDTO
                        {
                            Id = 1,
                            FirstName = "Ivo",
                            LastName = "Ivic",
                            Email = "iivic@example.com"
                        }
                    }
                }
                    }

                });

            var controller = new EventController(eventServiceMock.Object);

            // Act
            var actionResult = await controller.GetFullChargingHistory(1, 5, null);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as EventResponseDTO;
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("List of all events.", response.Message);
            Assert.NotNull(response.Events);
            Assert.Equal(1, response.Events.Count);
            Assert.Equal(response.Events[0].Id, 1);
            Assert.Equal(response.Events[0].Volume, 123);
            Assert.Equal("Ivo", response.Events[0].User.FirstName);
            Assert.Equal("Ivic", response.Events[0].User.LastName);
            Assert.Equal("iivic@example.com", response.Events[0].User.Email);
            Assert.Equal("Card 1", response.Events[0].Card.Name);
            Assert.Equal("RFID-ST34-56UV-7890", response.Events[0].Card.Value);
            Assert.Equal("New charger", response.Events[0].Charger.Name);
            Assert.Equal(50, response.Events[0].Charger.Latitude);
            Assert.Equal(40, response.Events[0].Charger.Longitude);
        }

        [Fact]
        public async Task GetFullChargingHistory_WhenEventServiceReturnsFailure_ShouldReturnBadRequest()
        {
            // Arrange
            var eventServiceMock = new Mock<IEventService>();
            eventServiceMock.Setup(service => service.GetFullChargingHistory(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(
                new EventResponseDTO
                {
                    Success = false,
                    Message = "There are no events with that parameters.",
                    Events = null
                });

            var controller = new EventController(eventServiceMock.Object);

            // Act
            var actionResult = await controller.GetFullChargingHistory(1, 5, null);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as EventResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("There are no events with that parameters.", response.Message);
            Assert.Null(response.Events);
        }

    }
}

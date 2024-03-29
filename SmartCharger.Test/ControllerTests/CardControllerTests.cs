﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Controllers;
using System.Net.Http;
using System.Security.Claims;

namespace SmartCharger.Test.ControllerTests
{
    public class CardControllerTests
    {
        [Fact]
        public async Task GetAllCards_WhenCardServiceReturnsSuccess_ShouldReturnOk()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.GetAllCards(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = true,
                    Message = "List of RFID cards with users.",
                    Cards = new List<CardDTO>
                    {
                        new CardDTO
                        {
                            Id = 1,
                            Name = "Card 1",
                            Value = "RFID-ST34-56UV-7890",
                            Active = true,
                            User = new UserDTO
                            {
                                Id = 1,
                                FirstName = "Ivo",
                                LastName = "Ivic",
                                Email = "iivic@example.com"
                            }
                        }
                    }
                });

            var controller = new CardController(cardServiceMock.Object);
            
            // Act
            var actionResult = await controller.GetAllCards();

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("List of RFID cards with users.", response.Message);
            Assert.Equal("1", response.Cards[0].Id.ToString());
        }

        [Fact]
        public async Task GetAllCards_WhenCardServiceReturnsNoCards_ShouldReturnBadRequest()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.GetAllCards(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = false,
                    Message = "There are no RFID cards with that parameters.",
                    Cards = null
                });

            var controller = new CardController(cardServiceMock.Object);

            // Act
            var actionResult = await controller.GetAllCards();

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("There are no RFID cards with that parameters.", response.Message);
            Assert.Null(response.Cards);
        }

        [Fact]
        public async Task GetCardById_WhenCardServiceReturnsSuccess_ShouldReturnOk()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.GetCardById(It.IsAny<int>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = true,
                    Message = "RFID card with user.",
                    Card = new CardDTO
                    {
                        Id = 1,
                        Name = "Card 1",
                        Value = "RFID-ST34-56UV-7890",
                        Active = true,
                        User = new UserDTO
                        {
                            Id = 1,
                            FirstName = "Ivo",
                            LastName = "Ivic",
                            Email = ""
                        }
                    }
                });

            var controller = new CardController(cardServiceMock.Object);

            // Act
            var actionResult = await controller.GetCardById(1);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("RFID card with user.", response.Message);
            Assert.Equal("1", response.Card.Id.ToString());
        }

        [Fact]
        public async Task GetCardById_WhenCardServiceReturnsNoCard_ShouldReturnBadRequest()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.GetCardById(It.IsAny<int>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = false,
                    Message = "There is no RFID card with that ID.",
                    Card = null
                });

            var controller = new CardController(cardServiceMock.Object);

            // Act
            var actionResult = await controller.GetCardById(1);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("There is no RFID card with that ID.", response.Message);
            Assert.Null(response.Card);
        }

        [Fact]
        public async Task UpdateActiveStatus_WhenCardServiceReturnsSuccess_ShouldReturnOk()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.UpdateActiveStatus(It.IsAny<int>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = true,
                    Message = "RFID card with ID:1 updated to False.",
                    Card = new CardDTO
                    {
                        Id = 1,
                        Name = "Card 1",
                        Value = "RFID-ST34-56UV-7890",
                        Active = false,
                        User = new UserDTO
                        {
                            Id = 1,
                            FirstName = "Ivo",
                            LastName = "Ivic",
                            Email = ""
                        }
                    }
                });

            var controller = new CardController(cardServiceMock.Object);

            // Act
            var actionResult = await controller.UpdateActiveStatus(1);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("RFID card with ID:1 updated to False.", response.Message);
            Assert.Equal("1", response.Card.Id.ToString());
            Assert.False(response.Card.Active);
        }

        [Fact]
        public async Task UpdateActiveStatus_WhenCardServiceReturnsNoCard_ShouldReturnBadRequest()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.UpdateActiveStatus(It.IsAny<int>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = false,
                    Message = "There is no RFID card with that ID.",
                    Card = null
                });

            var controller = new CardController(cardServiceMock.Object);

            // Act
            var actionResult = await controller.UpdateActiveStatus(1);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("There is no RFID card with that ID.", response.Message);
            Assert.Null(response.Card);
        }

        [Fact]
        public async Task DeleteCard_WhenCardServiceReturnsSuccess_ShouldReturnOk()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.DeleteCard(It.IsAny<int>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = true,
                    Message = "RFID card with ID:1 is deleted.",
                    Card = null
                });

            var controller = new CardController(cardServiceMock.Object);

            // Act
            var actionResult = await controller.DeleteCard(1);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("RFID card with ID:1 is deleted.", response.Message);
            Assert.Null(response.Card);
        }

        [Fact]
        public async Task DeleteCard_WhenCardServiceReturnsNoCard_ShouldReturnBadRequest()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.DeleteCard(It.IsAny<int>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = false,
                    Message = "There is no RFID card with that ID.",
                    Card = null
                });

            var controller = new CardController(cardServiceMock.Object);

            // Act
            var actionResult = await controller.DeleteCard(1);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("There is no RFID card with that ID.", response.Message);
            Assert.Null(response.Card);
        }

        [Fact]
        public async Task GetAllCardsForUser_WhenCardServiceReturnsSuccess_ShouldReturnOk()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.GetAllCardsForUser(It.IsAny<int>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = true,
                    Message = "List of RFID cards for user.",
                    Cards = new List<CardDTO>
                    {
                        new CardDTO
                        {
                            Id = 1,
                            Name = "Card 1",
                            Value = "RFID-ST34-56UV-7890",
                            Active = true,
                            User = new UserDTO
                            {
                                Id = 1,
                                FirstName = "Ivo",
                                LastName = "Ivic",
                                Email = ""
                            }
                        }
                    }
                });

            var controller = CreateCardControllerWithAuthenticatedUser(cardServiceMock, "1");

            // Act
            var actionResult = await controller.GetAllCardsForUser(1);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.True(response.Success);
            Assert.Equal("List of RFID cards for user.", response.Message);
            Assert.Equal("1", response.Cards[0].Id.ToString());
        }

        [Fact]
        public async Task GetAllCardsForUser_WhenCardServiceReturnsNoCards_ShouldReturnBadRequest()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.GetAllCardsForUser(It.IsAny<int>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = false,
                    Message = "User with ID:1 has no RFID card.",
                    Cards = null
                });

            var controller = CreateCardControllerWithAuthenticatedUser(cardServiceMock, "1");

            // Act
            var actionResult = await controller.GetAllCardsForUser(1);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.False(response.Success);
            Assert.Equal("User with ID:1 has no RFID card.", response.Message);
            Assert.Null(response.Cards);
        }

        [Fact]
        public async Task GetCardByIdForUser_WhenCardServiceReturnsSuccess_ShouldReturnOk()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.GetCardByIdForUser(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = true,
                    Message = "RFID card with user.",
                    Card = new CardDTO
                    {
                        Id = 1,
                        Name = "Card 1",
                        Value = "RFID-ST34-56UV-7890",
                        Active = true,
                        User = new UserDTO
                        {
                            Id = 1,
                            FirstName = "Ivo",
                            LastName = "Ivic",
                            Email = ""
                        }
                    }
                });

            var controller = CreateCardControllerWithAuthenticatedUser(cardServiceMock, "1");

            // Act
            var actionResult = await controller.GetCardByIdForUser(1, 1);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.True(response.Success);
            Assert.Equal("RFID card with user.", response.Message);
            Assert.Equal("1", response.Card.Id.ToString());
        }

        [Fact]
        public async Task GetCardByIdForUser_WhenCardServiceReturnsNoCard_ShouldReturnBadRequest()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.GetCardByIdForUser(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = false,
                    Message = "RFID card with ID:1 doesn't exist.",
                    Card = null
                });

            var controller = CreateCardControllerWithAuthenticatedUser(cardServiceMock, "1");

            // Act
            var actionResult = await controller.GetCardByIdForUser(1, 1);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.False(response.Success);
            Assert.Equal("RFID card with ID:1 doesn't exist.", response.Message);
            Assert.Null(response.Card);
        }

        [Fact]
        public async Task AddCard_WhenCardServiceReturnsSuccess_ShouldReturnOk()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.AddCard(It.IsAny<AddCardDTO>(), It.IsAny<int>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = true,
                    Message = "Successfully added RFID card.",
                    Card = new CardDTO
                    {
                        Id = 1,
                        Name = "Card 1",
                        Value = "RFID-ST34-56UV-7890",
                        Active = true,
                        User = new UserDTO
                        {
                            Id = 1,
                            FirstName = "Ivo",
                            LastName = "Ivic",
                            Email = ""
                        }
                    }
                });

            var controller = CreateCardControllerWithAuthenticatedUser(cardServiceMock, "1");

            // Act
            var actionResult = await controller.AddCard(1, new AddCardDTO
            {
                Name = "Card 1",
                Value = "RFID-ST34-56UV-7890"
            });

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.True(response.Success);
            Assert.Equal("Successfully added RFID card.", response.Message);
            Assert.Equal("1", response.Card.Id.ToString());
        }

        [Fact]
        public async Task AddCard_WhenCardServiceReturnsCardWithSameValueExists_ShouldReturnBadRequest()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.AddCard(It.IsAny<AddCardDTO>(), It.IsAny<int>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = false,
                    Message = "RFID card with same value already exists.",
                    Card = null
                });

            var controller = CreateCardControllerWithAuthenticatedUser(cardServiceMock, "1");

            // Act
            var actionResult = await controller.AddCard(1, new AddCardDTO
            {
                Name = "Card 1",
                Value = "RFID-ST34-56UV-7890"
            });

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.False(response.Success);
            Assert.Equal("RFID card with same value already exists.", response.Message);
            Assert.Null(response.Card);
        }

        [Fact]
        public async Task DeleteCardForUser_WhenCardServiceReturnsSuccess_ShouldReturnOk()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.DeleteCardForUser(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = true,
                    Message = "Successfully deleted RFID card with ID:1.",
                    Card = null
                });

            var controller = CreateCardControllerWithAuthenticatedUser(cardServiceMock, "1");

            // Act
            var actionResult = await controller.DeleteCardForUser(1, 1);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.True(response.Success);
            Assert.Equal("Successfully deleted RFID card with ID:1.", response.Message);
            Assert.Null(response.Card);
        }

        [Fact]
        public async Task DeleteCardForUser_WhenCardServiceReturnsCardDoesntExist_ShouldReturnBadRequest()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.DeleteCardForUser(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = false,
                    Message = "RFID card with ID:1 doesn't exist.",
                    Card = null
                });

            var controller = CreateCardControllerWithAuthenticatedUser(cardServiceMock, "1");

            // Act
            var actionResult = await controller.DeleteCardForUser(1, 1);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as CardsResponseDTO;
            Assert.False(response.Success);
            Assert.Equal("RFID card with ID:1 doesn't exist.", response.Message);
            Assert.Null(response.Card);
        }

        [Fact]
        public async Task VerifyCard_WhenCardServiceReturnsSuccess_ShouldReturnOk()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.VerifyCard(It.IsAny<string>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = true,
                    Message = "RFID card is accepted.",
                    Card = new CardDTO
                    {
                        Id = 1,
                        Name = "Card 1",
                        Value = "RFID-ST34-56UV-7890",
                        Active = true,
                        User = new UserDTO
                        {
                            Id = 1,
                            FirstName = "Ivo",
                            LastName = "Ivic",
                            Email = ""
                        }
                    }
                });

            var controller = new CardController(cardServiceMock.Object);

            // Act
            var actionResult = await controller.VerifyCard("RFID-ST34-56UV-7890");

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as ResponseBaseDTO;
            Assert.True(response.Success);
            Assert.Equal("RFID card is accepted.", response.Message);
        }

        [Fact]
        public async Task VerifyCard_WhenCardServiceReturnsFailure_ShouldReturnForbidden()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(service => service.VerifyCard(It.IsAny<string>()))
                .ReturnsAsync(new CardsResponseDTO
                {
                    Success = false,
                    Message = "RFID card with name Card 1 is not active."
                });

            var controller = new CardController(cardServiceMock.Object);

            // Act
            var actionResult = await controller.VerifyCard("RFID-ST34-56UV-7890");

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(403, result.StatusCode);
            var response = result.Value as ResponseBaseDTO;
            Assert.False(response.Success);
            Assert.Equal("RFID card with name Card 1 is not active.", response.Message);
        }

        private CardController CreateCardControllerWithAuthenticatedUser(Mock<ICardService> cardServiceMock, string userId)
        {
            var httpContext = new Mock<HttpContext>();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", userId)
            }));
            httpContext.Setup(c => c.User).Returns(user);

            var controller = new CardController(cardServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext.Object
                }
            };

            return controller;
        }
    }
}

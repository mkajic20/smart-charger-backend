using Microsoft.EntityFrameworkCore;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Services;
using SmartCharger.Data;
using SmartCharger.Data.Entities;

namespace SmartCharger.Test.ServicesTests
{
    public class CardServiceTests
    {
        [Fact]
        public async Task GetAllCards_WhenCardsExists_ShouldReturnListOfCards()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "CardServiceDatabase")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.GetAllCards();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("List of RFID cards with users.", result.Message);
                Assert.NotNull(result.Cards);
                Assert.Equal("1", result.Cards[0].Id.ToString());
                Assert.Equal("RFID-ST34-56UV-7890", result.Cards[0].Value);
                Assert.Equal("Ivo", result.Cards[0].User.FirstName);
                Assert.Equal("Ivic", result.Cards[0].User.LastName);
            }
        }

        [Fact]
        public async Task GetAllCards_WhenCardsDontExist_ShouldReturnEmptyListOfCards()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "Empty")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.GetAllCards();

                // Assert
                Assert.False(result.Success);
                Assert.Equal("There are no RFID cards with that parameters.", result.Message);
                Assert.Null(result.Cards);
            }
        }

        [Fact]
        public async Task GetCardById_WhenCardExists_ShouldReturnCard()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "CardServiceDatabase2")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.GetCardById(1);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("RFID-ST34-56UV-7890", result.Card.Value);
                Assert.Equal("Ivo", result.Card.User.FirstName);
                Assert.Equal("Ivic", result.Card.User.LastName);
            }
        }

        [Fact]
        public async Task GetCardById_WhenCardDoesntExist_ShouldReturnEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "Empty")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.GetCardById(1);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("There is no RFID card with that ID.", result.Message);
                Assert.Null(result.Cards);
            }
        }

        [Fact]
        public async Task UpdateActiveStatus_WhenCardExists_ShouldReturnCard()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "CardServiceDatabase3")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.UpdateActiveStatus(1);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("RFID-ST34-56UV-7890", result.Card.Value);
                Assert.False(result.Card.Active);
                Assert.Equal("Ivo", result.Card.User.FirstName);
                Assert.Equal("Ivic", result.Card.User.LastName);
            }
        }

        [Fact]
        public async Task UpdateActiveStatus_WhenCardDoesntExist_ShouldReturnEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "Empty")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.UpdateActiveStatus(1);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("There is no RFID card with that ID.", result.Message);
                Assert.Null(result.Cards);
            }
        }

        [Fact]
        public async Task DeleteCard_WhenCardExists_ShouldReturnSuccess()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "CardServiceDatabase4")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.DeleteCard(1);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("RFID card with ID:1 is deleted.", result.Message);
                Assert.Null(result.Card);
            }
        }

        [Fact]
        public async Task DeleteCard_WhenCardDoesntExist_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "Empty")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.DeleteCard(1);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("There is no RFID card with that ID.", result.Message);
                Assert.Null(result.Cards);
            }
        }

        [Fact]
        public async Task GetAllCardsForUser_WhenCardsExists_ShouldReturnListOfCards()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "CardServiceDatabase5")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.GetAllCardsForUser(1);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("List of RFID cards for user with ID:1.", result.Message);
                Assert.NotNull(result.Cards);
                Assert.Equal("1", result.Cards[0].Id.ToString());
                Assert.Equal("RFID-ST34-56UV-7890", result.Cards[0].Value);
                Assert.Equal("Ivo", result.Cards[0].User.FirstName);
                Assert.Equal("Ivic", result.Cards[0].User.LastName);
            }
        }

        [Fact]
        public async Task GetAllCardsForUser_WhenCardsDontExist_ShouldReturnEmptyListOfCards()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "Empty")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.GetAllCardsForUser(1);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("User with ID:1 has no RFID card.", result.Message);
                Assert.Null(result.Cards);
            }
        }

        [Fact]
        public async Task GetCardByIdForUser_WhenCardExists_ShouldReturnCard()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "CardServiceDatabase6")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.GetCardByIdForUser(1, 1);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("RFID-ST34-56UV-7890", result.Card.Value);
                Assert.Equal(1, result.Card.Id);
                Assert.Equal(1, result.Card.User.Id);
                Assert.Equal("Ivo", result.Card.User.FirstName);
                Assert.Equal("Ivic", result.Card.User.LastName);
            }
        }

        [Fact]
        public async Task GetCardByIdForUser_WhenCardDoesntExist_ShouldReturnEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "Empty")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.GetCardByIdForUser(1, 1);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("RFID card with ID:1 doesn't exist.", result.Message);
                Assert.Null(result.Card);
            }
        }

        [Fact]
        public async Task AddCard_WhenCardDoesntExist_ShouldReturnCard()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "CardServiceDatabase7")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);
                AddCardDTO card = new AddCardDTO
                {
                    Value = "RFID-NF51-42VA-4215",
                    Name = "Card 3",
                };

                // Act
                CardsResponseDTO result = await cardService.AddCard(card, 1);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("RFID-NF51-42VA-4215", result.Card.Value);
                Assert.Equal("Card 3", result.Card.Name);
                Assert.Equal(1, result.Card.User.Id);
                Assert.Equal("Ivo", result.Card.User.FirstName);
                Assert.Equal("Ivic", result.Card.User.LastName);
            }
        }

        [Fact]
        public async Task AddCard_WhenCardExists_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "CardServiceDatabase8")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);
                AddCardDTO card = new AddCardDTO
                {
                    Value = "RFID-ST34-56UV-7890",
                    Name = "Card 3",
                };

                // Act
                CardsResponseDTO result = await cardService.AddCard(card, 1);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("RFID card with same value already exists.", result.Message);
                Assert.Null(result.Card);
            }
        }

        [Fact]
        public async Task DeleteCardForUser_WhenCardExists_ShouldReturnSuccess()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "CardServiceDatabase9")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.DeleteCardForUser(1, 1);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("Successfully deleted RFID card with ID:1.", result.Message);
                Assert.Null(result.Card);
            }
        }

        [Fact]
        public async Task DeleteCardForUser_WhenCardDoesntExist_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "empty")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                var cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.DeleteCardForUser(1, 1);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("RFID card with ID:1 doesn't exist.", result.Message);
                Assert.Null(result.Card);
            }
        }

        [Fact]
        public async Task VerifyCard_WhenCardExistsAndReadyToUse_ShouldReturnSuccess()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "CardServiceDatabase10")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
            }

            using (var context = new SmartChargerContext(options))
            {
                CardService cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.VerifyCard("RFID-ST34-56UV-7890");

                // Assert
                Assert.True(result.Success);
                Assert.Equal("RFID card is accepted.", result.Message);
                Assert.Equal("RFID-ST34-56UV-7890", result.Card.Value);
            }
        }

        [Fact]
        public async Task VerifyCard_WhenCardExistsAndIsNotActive_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "CardServiceDatabase11")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
                context.Cards.FirstOrDefault().Active = false;
                context.SaveChanges();
            }

            using (var context = new SmartChargerContext(options))
            {
                CardService cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.VerifyCard("RFID-ST34-56UV-7890");

                // Assert
                Assert.False(result.Success);
                Assert.Equal("RFID card with name Card 1 is not active.", result.Message);
            }
        }

        [Fact]
        public async Task VerifyCard_WhenCardExistsAndIsInUse_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "CardServiceDatabase12")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                SetupDatabase(context);
                context.Cards.FirstOrDefault().UsageStatus = true;
                context.SaveChanges();
            }

            using (var context = new SmartChargerContext(options))
            {
                CardService cardService = new CardService(context);

                // Act
                CardsResponseDTO result = await cardService.VerifyCard("RFID-ST34-56UV-7890");

                // Assert
                Assert.False(result.Success);
                Assert.Equal("RFID card with name Card 1 is already in use.", result.Message);
            }
        }

        [Fact]
        public async Task VerifyCard_WhenCardDoesntExist_ShouldReturnFailure()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "empty")
                .Options;

            using (var context = new SmartChargerContext(options))
            {
                CardService cardService = new CardService(context);

                // Act
                ResponseBaseDTO result = await cardService.VerifyCard("RFID-ST34-56UV-7890");

                // Assert
                Assert.False(result.Success);
                Assert.Equal("RFID card with that value doesn't exist.", result.Message);
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
                Id = 2,
                FirstName = "Mirko",
                LastName = "Miric",
                Email = "mmiric@example.com",
                Password = hashedPassword,
                Active = false,
                CreationTime = DateTime.Now.ToUniversalTime(),
                Salt = salt,
                RoleId = 2
            });

            context.Cards.Add(new Card
            {
                Value = "RFID-ST34-56UV-7890",
                Active = true,
                Name = "Card 1",
                UserId = 1
            });

            context.Cards.Add(new Card
            {
                Value = "RFID-OP56-78QR-9012",
                Active = true,
                Name = "Card 2",
                UserId = 1
            });

            context.Cards.Add(new Card
            {
                Value = "RFID-DE32-72FJ-3821",
                Active = true,
                Name = "Card 1",
                UserId = 2
            });

            context.SaveChanges();
        }
    }
}

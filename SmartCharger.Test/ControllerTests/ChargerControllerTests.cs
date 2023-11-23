

using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Controllers;

namespace SmartCharger.Test.ControllerTests
{
    public class ChargerControllerTests
    {

        [Fact]
        public async Task GetAllChargers_WhenChargerServiceReturnsListOfChargers_ShouldReturnOk()
        {
            //Arrange
            var chargerServiceMock = new Mock<IChargerService>();
            chargerServiceMock.Setup(service => service.GetAllChargers(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new ChargerResponseDTO
            {
                Success = true,
                Message = "List of chargers.",
                Chargers = new List<ChargerDTO>
                {
                    new ChargerDTO
                    {
                        Id = 1,
                        Name = "Charger 1",
                        Latitude = 55,
                        Longitude = 50,
                    }
                }
            });

            var controller = new ChargerController(chargerServiceMock.Object);

            //Act
            var actionResult = await controller.GetAllChargers();

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as ChargerResponseDTO;
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("List of chargers.", response.Message);
            Assert.Equal(1, response.Chargers[0].Id);
            Assert.Equal(55, response.Chargers[0].Latitude);
            Assert.Equal(50, response.Chargers[0].Longitude);
            Assert.Equal("Charger 1", response.Chargers[0].Name);


        }

        [Fact]
        public async Task GetAllChargers_WhenChargerServiceReturnsNoChargers_ShouldReturnBadRequest()
        {
            //Arrange
            var chargerServiceMock = new Mock<IChargerService>();
            chargerServiceMock.Setup(service => service.GetAllChargers(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new ChargerResponseDTO
            {
                Success = false,
                Message = "There are no chargers with that parameters.",
                Chargers = null
            });

            var controller = new ChargerController(chargerServiceMock.Object);

            //Act
            var actionResult = await controller.GetAllChargers();

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as ChargerResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("There are no chargers with that parameters.", response.Message);
            Assert.Null(response.Chargers);


        }
        [Fact]
        public async Task CreateNewCharger_WhenChargerValid_ShouldReturnOk()
        {
            //Arrange
            var chargerServiceMock = new Mock<IChargerService>();
            chargerServiceMock.Setup(x => x.CreateNewCharger(It.IsAny<ChargerDTO>()))
                .ReturnsAsync(new ChargerResponseDTO
                {
                    Success = true,
                    Message = "Charger created successfully.",
                    Charger = new ChargerDTO
                    {
                        Id = 1,
                        Name = "Charger 1",
                        Latitude = 55,
                        Longitude = -11,
                        CreatorId = 1
                    }
                });

            var controller = new ChargerController(chargerServiceMock.Object);

            //Act
            var actionResult = await controller.CreateCharger(new ChargerDTO
            {
                Id = 1,
                Name = "Charger 1",
                Latitude = 55,
                Longitude = -11,
                CreatorId = 1
            });

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as ChargerResponseDTO;
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("Charger created successfully.", response.Message);
            Assert.Equal(55, response.Charger.Latitude);
            Assert.Equal(-11, response.Charger.Longitude);
            Assert.Equal("Charger 1", response.Charger.Name);


        }


        [Fact]
        public async Task CreateNewCharger_WhenChargersLatitudeInvalid_ShouldReturnBadRequest()
        {
            var chargerServiceMock = new Mock<IChargerService>();
            chargerServiceMock.Setup(x => x.CreateNewCharger(It.IsAny<ChargerDTO>()))
                .ReturnsAsync(new ChargerResponseDTO
                {
                    Success = false,
                    Message = "Charger creation failed.",
                    Error = "Latitude must be between -90 and 90.",
                    Charger = null
                });

            var controller = new ChargerController(chargerServiceMock.Object);

            //Act
            var actionResult = await controller.CreateCharger(new ChargerDTO
            {
                Id = 1,
                Name = "New charger",
                Latitude = 5555,
                Longitude = -11,
                CreatorId = 1
            });

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as ChargerResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("Charger creation failed.", response.Message);
            Assert.Equal("Latitude must be between -90 and 90.", response.Error);
            Assert.Null(response.Chargers);


        }

        [Fact]
        public async Task CreateNewCharger_WhenChargersLongitudeInvalid_ShouldReturnBadRequest()
        {
            var chargerServiceMock = new Mock<IChargerService>();
            chargerServiceMock.Setup(x => x.CreateNewCharger(It.IsAny<ChargerDTO>()))
                .ReturnsAsync(new ChargerResponseDTO
                {
                    Success = false,
                    Message = "Charger creation failed.",
                    Error = "Longitude must be between -180 and 180.",
                    Charger = null
                });

            var controller = new ChargerController(chargerServiceMock.Object);

            //Act
            var actionResult = await controller.CreateCharger(new ChargerDTO
            {
                Id = 1,
                Name = "New charger",
                Latitude = 55,
                Longitude = -1111,
                CreatorId = 1
            });

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as ChargerResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("Charger creation failed.", response.Message);
            Assert.Equal("Longitude must be between -180 and 180.", response.Error);
            Assert.Null(response.Chargers);


        }

        [Fact]
        public async Task CreateNewCharger_WhenChargersNameInvalid_ShouldReturnBadRequest()
        {
            var chargerServiceMock = new Mock<IChargerService>();
            chargerServiceMock.Setup(x => x.CreateNewCharger(It.IsAny<ChargerDTO>()))
                .ReturnsAsync(new ChargerResponseDTO
                {
                    Success = false,
                    Message = "Charger creation failed.",
                    Error = "Name of the charger cannot be empty.",
                    Charger = null
                });

            var controller = new ChargerController(chargerServiceMock.Object);

            //Act
            var actionResult = await controller.CreateCharger(new ChargerDTO
            {
                Id = 1,
                Name = "",
                Latitude = 55,
                Longitude = 11,
                CreatorId = 1
            });

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as ChargerResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("Charger creation failed.", response.Message);
            Assert.Equal("Name of the charger cannot be empty.", response.Error);
            Assert.Null(response.Charger);


        }

        [Fact]
        public async Task DeleteCharger_WhenChargerServiceReturnsSuccess_ShouldReturnOk()
        {
            //Arrange
            var chargerServiceMock = new Mock<IChargerService>();
            chargerServiceMock.Setup(x => x.DeleteCharger(It.IsAny<int>()))
                .ReturnsAsync(new ChargerResponseDTO
                {
                    Success = true,
                    Message = "Charger deleted successfully.",
                    Charger = null
                });

            var controller = new ChargerController(chargerServiceMock.Object);

            //Act
            var actionResult = await controller.DeleteCharger(1);

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as ChargerResponseDTO;
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("Charger deleted successfully.", response.Message);
            Assert.Null(response.Charger);
        }


        [Fact]
        public async Task DeleteCharger_WhenChargerServiceReturnsNoCharger_ShouldReturnBadRequest()
        {
            //Arrange
            var chargerServiceMock = new Mock<IChargerService>();
            chargerServiceMock.Setup(x => x.DeleteCharger(It.IsAny<int>()))
                .ReturnsAsync(new ChargerResponseDTO
                {
                    Success = false,
                    Message = "Unsuccessful deletion of the charger.",
                    Error = "Charger not found.",
                });

            var controller = new ChargerController(chargerServiceMock.Object);

            //Act
            var actionResult = await controller.DeleteCharger(1);

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as ChargerResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("Unsuccessful deletion of the charger.", response.Message);
            Assert.Equal("Charger not found.", response.Error);
        }

        [Fact]
        public async Task UpdateCharger_WhenChargerValid_ShouldReturnOk()
        {

            var chargerId = 1;
            var chargerToUpdate = new ChargerDTO
            {
                Id = 1,
                Name = "Updated charger",
                Latitude = 55,
                Longitude = 33,
                CreatorId = 1
            };
            var chargerServiceMock = new Mock<IChargerService>();
            chargerServiceMock
            .Setup(s => s.UpdateCharger(chargerId, chargerToUpdate))
            .ReturnsAsync(new ChargerResponseDTO
            {
                Success = true,
                Message = "Charger updated successfully.",
                Charger = chargerToUpdate

            });
            var controller = new ChargerController(chargerServiceMock.Object);
            var actionResult = await controller.UpdateCharger(chargerId, chargerToUpdate);

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult as ObjectResult;
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as ChargerResponseDTO;
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("Charger updated successfully.", response.Message);
            Assert.NotNull(response.Charger);
        }

        [Fact]
        public async Task UpdateCharger_WhenChargerDoesntExist_ShouldReturnBadRequest()
        {

            var chargerId = 0;
            var chargerToUpdate = new ChargerDTO
            {
                Id = 1,
                Name = "Updated charger",
                Latitude = 55,
                Longitude = 33,
                CreatorId = 1
            };
            var chargerServiceMock = new Mock<IChargerService>();
            chargerServiceMock
            .Setup(s => s.UpdateCharger(chargerId, chargerToUpdate))
            .ReturnsAsync(new ChargerResponseDTO
            {
                Success = false,
                Message = "Unsuccessful update of the charger. ",
                Error = "Charger not found."


            });
            var controller = new ChargerController(chargerServiceMock.Object);
            var actionResult = await controller.UpdateCharger(chargerId, chargerToUpdate);

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as ChargerResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("Unsuccessful update of the charger. ", response.Message);
            Assert.Equal("Charger not found.", response.Error);

        }
        [Fact]
        public async Task UpdateCharger_WhenChargersLatitudeInvalid_ShouldReturnBadRequest()
        {

            var chargerId = 1;
            var chargerToUpdate = new ChargerDTO
            {
                Id = 1,
                Name = "Updated charger",
                Latitude = 555,
                Longitude = 33,
                CreatorId = 1
            };
            var chargerServiceMock = new Mock<IChargerService>();
            chargerServiceMock
            .Setup(s => s.UpdateCharger(chargerId, chargerToUpdate))
            .ReturnsAsync(new ChargerResponseDTO
            {
                Success = false,
                Message = "Charger update failed.",
                Error = "Latitude must be between -90 and 90."


            });
            var controller = new ChargerController(chargerServiceMock.Object);
            var actionResult = await controller.UpdateCharger(chargerId, chargerToUpdate);

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as ChargerResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("Charger update failed.", response.Message);
            Assert.Equal("Latitude must be between -90 and 90.", response.Error);

        }

        [Fact]
        public async Task UpdateCharger_WhenChargersLongitudeInvalid_ShouldReturnBadRequest()
        {

            var chargerId = 1;
            var chargerToUpdate = new ChargerDTO
            {
                Id = 1,
                Name = "Updated charger",
                Latitude = 55,
                Longitude = 333,
                CreatorId = 1
            };
            var chargerServiceMock = new Mock<IChargerService>();
            chargerServiceMock
            .Setup(s => s.UpdateCharger(chargerId, chargerToUpdate))
            .ReturnsAsync(new ChargerResponseDTO
            {
                Success = false,
                Message = "Charger update failed.",
                Error = "Longitude must be between -180 and 180."


            });
            var controller = new ChargerController(chargerServiceMock.Object);
            var actionResult = await controller.UpdateCharger(chargerId, chargerToUpdate);

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as ChargerResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("Charger update failed.", response.Message);
            Assert.Equal("Longitude must be between -180 and 180.", response.Error);

        }

        [Fact]
        public async Task UpdateCharger_WhenChargersNameInvalid_ShouldReturnBadRequest()
        {

            var chargerId = 1;
            var chargerToUpdate = new ChargerDTO
            {
                Id = 1,
                Name = "",
                Latitude = 55,
                Longitude = 333,
                CreatorId = 1
            };
            var chargerServiceMock = new Mock<IChargerService>();
            chargerServiceMock
            .Setup(s => s.UpdateCharger(chargerId, chargerToUpdate))
            .ReturnsAsync(new ChargerResponseDTO
            {
                Success = false,
                Message = "Charger update failed.",
                Error = "Name of the charger cannot be empty."


            });
            var controller = new ChargerController(chargerServiceMock.Object);
            var actionResult = await controller.UpdateCharger(chargerId, chargerToUpdate);

            //Assert
            Assert.NotNull(actionResult);
            var result = actionResult as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as ChargerResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("Charger update failed.", response.Message);
            Assert.Equal("Name of the charger cannot be empty.", response.Error);

        }
    }
}

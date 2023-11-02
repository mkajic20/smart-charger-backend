using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharger.Test.ControllerTests
{
    public class LoginControllerTests
    {
        [Fact]
        public async Task Login_WhenLoginServiceReturnsSuccess_ShouldReturnOk()
        {
            // Arrange
            var loginServiceMock = new Mock<ILoginService>();
            loginServiceMock.Setup(service => service.LoginAsync(It.IsAny<LoginDTO>()))
                .ReturnsAsync(new LoginResponseDTO 
                {
                    Success = true,
                    Message = "Login successful.",
                    User = new UserDTO
                    {
                        FirstName = "Ivo",
                        LastName = "Ivic",
                        Email = "iivic@example.com",
                    },
                    Token = "token"
                });

            var controller = new LoginController(loginServiceMock.Object);
            var loginDTO = new LoginDTO();

            // Act
            var result = await controller.Login(loginDTO) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Login_WhenLoginServiceReturnsFailure_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginServiceMock = new Mock<ILoginService>();
            loginServiceMock.Setup(service => service.LoginAsync(It.IsAny<LoginDTO>()))
                .ReturnsAsync(new LoginResponseDTO
                {
                    Success = false,
                    Message = "Login failed.",
                    Error = "Invalid credentials."
                });

            var controller = new LoginController(loginServiceMock.Object);
            var loginDTO = new LoginDTO();

            // Act
            var result = await controller.Login(loginDTO) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }
    }
}

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
    public class RegisterControllerTests
    {
        [Fact]
        public async Task Register_WhenValidRegistration_ShouldReturnOk()
        {
            // Arrange
            var registerServiceMock = new Mock<IRegisterService>();
            registerServiceMock.Setup(service => service.RegisterAsync(It.IsAny<RegisterDTO>()))
                .ReturnsAsync(new RegisterResponseDTO
                {
                    Success = true,
                    Message = "Registration successful",
                    User = new RegisterDTO
                    {
                        FirstName = "Ivo",
                        LastName = "Ivic",
                        Email = "iivic@example.com"
                    }
                });

            var controller = new RegisterController(registerServiceMock.Object);

            var registerDTO = new RegisterDTO
            {
                FirstName = "Ivo",
                LastName = "Ivic",
                Email = "iivic@example.com",
                Password = "password123"
            };

            // Act
            var result = await controller.Register(registerDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.True(okResult.StatusCode == 200);
        }

        [Fact]
        public async Task Register_WhenInvalidRegistration_ShouldReturnBadRequest()
        {
            // Arrange
            var registerServiceMock = new Mock<IRegisterService>();
            registerServiceMock.Setup(service => service.RegisterAsync(It.IsAny<RegisterDTO>()))
                .ReturnsAsync(new RegisterResponseDTO
                {
                    Success = false,
                    Message = "Registration failed",
                    Error = "Password must have at least 6 characters."
                });

            var controller = new RegisterController(registerServiceMock.Object);

            var registerDTO = new RegisterDTO
            {
                FirstName = "Ivo",
                LastName = "Ivic",
                Email = "iivic@example.com",
                Password = "pass"
            };

            // Act
            var result = await controller.Register(registerDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.True(badRequestResult.StatusCode == 400);
        }
    }
}

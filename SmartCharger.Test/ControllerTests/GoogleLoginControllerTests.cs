

using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Controllers;

namespace SmartCharger.Test.ControllerTests
{
    public class GoogleLoginControllerTests
    {
        [Fact]
        public async Task LoginWithGoogle_WhenGoogleLoginServiceReturnsSuccess_ShouldReturnOk()
        {
            // Arrange
            var googleLoginServiceMock = new Mock<IGoogleLoginService>();
            googleLoginServiceMock.Setup(service => service.LoginWithGoogleAsync(It.IsAny<string>()))
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

            var controller = new GoogleLoginController(googleLoginServiceMock.Object);
            var accessToken = "google-access-token";

            // Act
            var result = await controller.LoginWithGoogle(accessToken) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task LoginWithGoogle_WhenGoogleLoginServiceReturnsFailure_ShouldReturnUnauthorized()
        {
            // Arrange
            var googleLoginServiceMock = new Mock<IGoogleLoginService>();
            googleLoginServiceMock.Setup(service => service.LoginWithGoogleAsync(It.IsAny<string>()))
                .ReturnsAsync(new LoginResponseDTO
                {
                    Success = false,
                    Message = "Login failed.",
                });

            var controller = new GoogleLoginController(googleLoginServiceMock.Object);
            var accessToken = "invalid-google-access-token";

            // Act
            var result = await controller.LoginWithGoogle(accessToken) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }
    }
}

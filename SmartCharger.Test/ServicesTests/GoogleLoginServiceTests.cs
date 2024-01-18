using Microsoft.EntityFrameworkCore;
using Moq;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Business.Services;
using SmartCharger.Data;

namespace SmartCharger.Test.ServicesTests
{
    public class GoogleLoginServiceTests
    {
        [Fact]
        public async Task LoginWithGoogleAsync_WhenUserProvidesValidAccessToken_ShouldReturnSuccess()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "GoogleLoginServiceDatabase")
                .Options;

            var googleAuthServiceMock = new Mock<IGoogleAuthService>();
            googleAuthServiceMock.Setup(service => service.GetUserInfoFromAuthCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(new LoginResponseDTO
                {
                    Success = true,
                    User = new UserDTO
                    {
                        Email = "iivic@example.com",
                        FirstName = "Ivo",
                        LastName = "Ivic"
                    }
                });

            using (var context = new SmartChargerContext(options))
            {
                var googleLoginService = new GoogleLoginService(context, googleAuthServiceMock.Object);

                // Act
                LoginResponseDTO result = await googleLoginService.LoginWithGoogleAsync("valid_access_token");

                // Assert
                Assert.True(result.Success);
                Assert.Equal("Login successful.", result.Message);
                Assert.Equal("iivic@example.com", result.User.Email);
                Assert.NotNull(result.Token);
            }
        }

        [Fact]
        public async Task LoginWithGoogleAsync_WhenUserPoricdesnvalidAccessToken_ShouldReturnError()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SmartChargerContext>()
                .UseInMemoryDatabase(databaseName: "GoogleLoginServiceDatabase")
                .Options;

            var googleAuthServiceMock = new Mock<IGoogleAuthService>();
            googleAuthServiceMock.Setup(service => service.GetUserInfoFromAuthCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(new LoginResponseDTO
                {
                    Success = false,
                    Message = "Invalid access token."
                });

            using (var context = new SmartChargerContext(options))
            {
                var googleLoginService = new GoogleLoginService(context, googleAuthServiceMock.Object);

                // Act
                LoginResponseDTO result = await googleLoginService.LoginWithGoogleAsync("invalid_access_token");

                // Assert
                Assert.False(result.Success);
                Assert.Equal("Invalid access token.", result.Message);
                Assert.Null(result.User);
                Assert.Null(result.Token);
            }
        }
    }
}

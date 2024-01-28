
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Controllers;

namespace SmartCharger.Test.ControllerTests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetAllUsers_WhenUserServiceReturnsListOfUsers_ShouldReturnOk()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.GetAllUsers(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new UsersResponseDTO
                {
                    Success = true,
                    Message = "List of users.",
                    Users = new List<UserDTO>
                    {
                new UserDTO
                {
                    Id = 1,
                    FirstName = "Ivo",
                    LastName = "Ivic",
                    Email = "iivic@example.com",
                    Active = true,
                    RoleId = 2
                }
                    }
                });

            var controller = new UserController(userServiceMock.Object);

            // Act
            var actionResult = await controller.GetAllUsers();

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetAllUsers_WhenUserServiceReturnsNoUsers_ShouldReturnBadRequest()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.GetAllUsers(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new UsersResponseDTO
                {
                    Success = false,
                    Message = "There are no users with that parameters.",
                    Users = null
                });

            var controller = new UserController(userServiceMock.Object);

            // Act
            var actionResult = await controller.GetAllUsers();

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.Equal(400, result.StatusCode);
            var response = result.Value as UsersResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Equal("There are no users with that parameters.", response.Message);
            Assert.Null(response.Users);
        }

        [Fact]
        public async Task GetUserById_WhenUserServiceReturnsUser_ShouldReturnOk()
        {
            // Arrange
            var userId = 1;
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.GetUserById(userId))
                .ReturnsAsync(new SingleUserResponseDTO
                {
                    Success = true,
                    Message = "User found.",
                    User = new UserDTO
                    {
                        Id = userId,
                        FirstName = "Ivo",
                        LastName = "Ivic",
                        Email = "iivic@example.com",
                        Active = true,
                        RoleId = 1
                    }
                });

            var controller = new UserController(userServiceMock.Object);

            // Act
            var actionResult = await controller.GetUserById(userId);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }


        [Fact]
        public async Task GetUserById_WhenUserNotFound_ShouldReturnError()
        {
            // Arrange
            var userId = 1;
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.GetUserById(userId))
                .ReturnsAsync(new SingleUserResponseDTO
                {
                    Success = false,
                    Message = "User not found.",
                    User = null
                });

            var controller = new UserController(userServiceMock.Object);

            // Act
            var actionResult = await controller.GetUserById(userId);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            var response = result.Value as SingleUserResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Null(response.User);
            Assert.Equal("User not found.", response.Message);
        }


        [Fact]
        public async Task UpdateActiveStatus_WhenUserServiceSuccessfullyUpdatesUser_ShouldReturnOk()
        {
            // Arrange
            var userId = 1;
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.UpdateActiveStatus(userId))
                .ReturnsAsync(new SingleUserResponseDTO
                {
                    Success = true,
                    Message = "User activated/deactivated.",
                    User = new UserDTO
                    {
                        Id = userId,
                        FirstName = "Ivo",
                        LastName = "Ivic",
                        Email = "iivic@example.com",
                        Active = false,
                        RoleId = 1
                    }
                });

            var controller = new UserController(userServiceMock.Object);

            // Act
            var actionResult = await controller.UpdateActiveStatus(userId);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task UpdateActiveStatus_WhenUserNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var userId = 1;
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.UpdateActiveStatus(userId))
                .ReturnsAsync(new SingleUserResponseDTO
                {
                    Success = false,
                    Message = "User not found.",
                    User = null
                });

            var controller = new UserController(userServiceMock.Object);

            // Act
            var actionResult = await controller.UpdateActiveStatus(userId);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as NotFoundObjectResult;
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);

            var response = result.Value as SingleUserResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Null(response.User);
            Assert.Equal("User not found.", response.Message);
        }

        [Fact]
        public async Task UpdateRole_WhenUserServiceSuccessfullyChangeRole_ShouldReturnOk()
        {
            // Arrange
            var userId = 1;
            var roleId = 2;
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.UpdateRole(userId, roleId))
                .ReturnsAsync(new SingleUserResponseDTO
                {
                    Success = true,
                    Message = "User's role has been updated.",
                    User = new UserDTO
                    {
                        Id = userId,
                        FirstName = "Ivo",
                        LastName = "Ivic",
                        Email = "iivic@example.com",
                        Active = true,
                        RoleId = 2
                    }
                });

            var controller = new UserController(userServiceMock.Object);

            // Act
            var actionResult = await controller.UpdateRole(userId, roleId);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task UpdateRole_WhenUserNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var userId = 1;
            var roleId = 2;
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.UpdateRole(userId, roleId))
                .ReturnsAsync(new SingleUserResponseDTO
                {
                    Success = false,
                    Message = "User not found.",
                    User = null
                });

            var controller = new UserController(userServiceMock.Object);

            // Act
            var actionResult = await controller.UpdateRole(userId, roleId);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as NotFoundObjectResult;
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);

            var response = result.Value as SingleUserResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.Null(response.User);
            Assert.Equal("User not found.", response.Message);
        }

        [Fact]
        public async Task UpdateRole_WhenUserHasTheSameRoleHeUpdates_ShouldReturnConflict()
        {
            // Arrange
            var userId = 1;
            var roleId = 2;
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.UpdateRole(userId, roleId))
                .ReturnsAsync(new SingleUserResponseDTO
                {
                    Success = false,
                    Message = "User Ivo Ivic's role is already set to that role. No changes made.",
                    User = new UserDTO
                    {
                        Id = userId,
                        FirstName = "Ivo",
                        LastName = "Ivic",
                        Email = "iivic@example.com",
                        Active = true,
                        RoleId = 2
                    }
                });

            var controller = new UserController(userServiceMock.Object);

            // Act
            var actionResult = await controller.UpdateRole(userId, roleId);

            // Assert
            Assert.NotNull(actionResult);
            var result = actionResult.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(409, result.StatusCode);

            var response = result.Value as SingleUserResponseDTO;
            Assert.NotNull(response);
            Assert.False(response.Success);
            Assert.NotNull(response.User);
            Assert.Equal("User Ivo Ivic's role is already set to that role. No changes made.", response.Message);
        }
    }
}


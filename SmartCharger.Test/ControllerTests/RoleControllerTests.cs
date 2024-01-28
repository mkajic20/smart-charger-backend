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
        public class RoleControllerTests
        {
            [Fact]
            public async Task GetAllRoles_WhenRoleServiceReturnsListOfRoles_ShouldReturnOk()
            {
                // Arrange
                var roleServiceMock = new Mock<IRoleService>();
                roleServiceMock.Setup(service => service.GetAllRoles())
                    .ReturnsAsync(new RoleResponseDTO
                    {
                        Success = true,
                        Message = "List of roles.",
                        Roles = new List<RoleDTO>
                        {
                        new RoleDTO
                        {
                            Id = 1,
                            Name = "Admin"
                        }
                        }
                    });

                var controller = new RoleController(roleServiceMock.Object);

                // Act
                var actionResult = await controller.GetAllRoles();

                // Assert
                Assert.NotNull(actionResult);
                var result = actionResult.Result as ObjectResult;
                Assert.NotNull(result);

                var response = result.Value as RoleResponseDTO;
                Assert.NotNull(response);
                Assert.Equal(1, response.Roles[0].Id);
                Assert.Equal("Admin", response.Roles[0].Name);
        }

        }
    }


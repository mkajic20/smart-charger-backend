using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;

namespace SmartCharger.Controllers
{
    [Route("api/admin/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [Authorize(Policy = "Admin")]
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string search = null)
        {

            UsersResponseDTO response = await _userService.GetAllUsers(page, pageSize, search);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int userId)
        {

            SingleUserResponseDTO response = await _userService.GetUserById(userId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch("users/{userId}/activate")]
        public async Task<ActionResult<UserDTO>> UpdateActiveStatus(int userId)
        {
            SingleUserResponseDTO response = await _userService.UpdateActiveStatus(userId);
            if (response.Success == false)
            {
                if (response.Message == "User not found.")
                {
                    return NotFound(response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }

            return Ok(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch("users/{userId}/role/{roleId}")]
        public async Task<ActionResult<UserDTO>> UpdateRole(int userId, int roleId)
        {
            SingleUserResponseDTO response = await _userService.UpdateRole(userId, roleId);

            if (!response.Success)
            {
                if (response.Message == "User not found.")
                {
                    return NotFound(response);
                }
                else if (response.Message.Contains("role is already set"))
                {
                    return StatusCode(StatusCodes.Status409Conflict, response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }

            return Ok(response);
        }


    }


}

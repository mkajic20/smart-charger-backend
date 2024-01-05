using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Business.Services;

namespace SmartCharger.Controllers
{
    [Route("api/")]
    [ApiController]
    public class EventController : ControllerBase
    {

        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [Authorize(Policy = "AdminOrCustomer")]
        [HttpGet("users/{userId}/history")]
        public async Task<ActionResult<IEnumerable<EventResponseDTO>>> GetUsersChargingHistory(int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 5, [FromQuery] string search = null)
        {
            EventResponseDTO response = await _eventService.GetUsersChargingHistory(userId, page, pageSize, search);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("events/start")]
        public async Task<ActionResult<IEnumerable<EventResponseDTO>>> StartCharging([FromBody] EventChargingDTO eventDTO)
        {
            EventResponseDTO response = await _eventService.StartCharging(eventDTO);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPatch("events/stop")]
        public async Task<ActionResult<IEnumerable<EventResponseDTO>>> EndCharging([FromBody] EventChargingDTO eventDTO)
        {
            EventResponseDTO response = await _eventService.EndCharging(eventDTO);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        [Authorize(Policy = "Admin")]
        [HttpGet("admin/history")]
        public async Task<ActionResult<IEnumerable<EventResponseDTO>>> GetFullUsersChargingHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string search = null)
        {
            EventResponseDTO response = await _eventService.GetFullChargingHistory(page, pageSize, search);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}

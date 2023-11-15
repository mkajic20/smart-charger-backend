using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Business.Services;

namespace SmartCharger.Controllers
{
    [Route("api/admin/")]
    [ApiController]
    public class ChargerController : ControllerBase
    {
        private readonly IChargerService _chargerService;

        public ChargerController(IChargerService chargerService)
        {
            _chargerService = chargerService;
        }


        [Authorize(Policy = "Admin")]
        [HttpPost("chargers")]
        public async Task<IActionResult> CreateCharger([FromBody] ChargerDTO chargerDTO)
        {
            ChargerResponseDTO response = await _chargerService.CreateNewCharger(chargerDTO);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("chargers/{chargerId}")]
        public async Task<IActionResult> DeleteCharger(int chargerId)
        {
            ChargerResponseDTO response = await _chargerService.DeleteCharger(chargerId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}

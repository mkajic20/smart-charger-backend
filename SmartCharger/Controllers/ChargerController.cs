﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Business.Services;

namespace SmartCharger.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ChargerController : ControllerBase
    {
        private readonly IChargerService _chargerService;

        public ChargerController(IChargerService chargerService)
        {
            _chargerService = chargerService;
        }


        [HttpGet("chargers")]
        public async Task<ActionResult<IEnumerable<ChargerResponseDTO>>> GetAllChargers([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string search = null)
        {
            ChargerResponseDTO response = await _chargerService.GetAllChargers(page, pageSize, search);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("admin/chargers")]
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
        [HttpDelete("admin/chargers/{chargerId}")]
        public async Task<IActionResult> DeleteCharger(int chargerId)
        {
            ChargerResponseDTO response = await _chargerService.DeleteCharger(chargerId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("admin/chargers/{chargerId}")]
        public async Task<IActionResult> UpdateCharger(int chargerId, [FromBody]ChargerDTO chargerDTO)
        {
            ChargerResponseDTO response = await _chargerService.UpdateCharger(chargerId, chargerDTO);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("admin/chargers/{chargerId}")]
        public async Task<ActionResult<IEnumerable<ChargerResponseDTO>>> GetChargerById(int chargerId)
        {
            ChargerResponseDTO response = await _chargerService.GetChargerById(chargerId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}

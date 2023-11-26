using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Business.Services;
using System.Security.Claims;

namespace SmartCharger.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("admin/cards")]
        public async Task<ActionResult<IEnumerable<CardsResponseDTO>>> GetAllCards([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string search = null)
        {
            CardsResponseDTO response = await _cardService.GetAllCards(page, pageSize, search);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("admin/cards/{cardId}")]
        public async Task<ActionResult<IEnumerable<CardsResponseDTO>>> GetCardById(int cardId)
        {
            CardsResponseDTO response = await _cardService.GetCardById(cardId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch("admin/cards/{cardId}/active")]
        public async Task<ActionResult<IEnumerable<CardsResponseDTO>>> UpdateActiveStatus(int cardId)
        {
            CardsResponseDTO response = await _cardService.UpdateActiveStatus(cardId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("admin/cards/{cardId}")]
        public async Task<ActionResult<IEnumerable<CardsResponseDTO>>> DeleteCard(int cardId)
        {
            CardsResponseDTO response = await _cardService.DeleteCard(cardId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("/users/{userId}/cards")]
        public async Task<ActionResult<IEnumerable<CardsResponseDTO>>> GetAllCards(int userId)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if(userIdClaim != userId.ToString())
            {
                return Forbid();
            }

            CardsResponseDTO response = await _cardService.GetAllCardsForUser(userId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}

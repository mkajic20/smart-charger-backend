using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;

namespace SmartCharger.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            RegisterResponseDTO registrationResult = await _registerService.RegisterAsync(registerDTO);

            if (registrationResult.Success == true)
            {
                return Ok(registrationResult);
            }
            else
            {
                return BadRequest(registrationResult);
            }
        }
    }
}
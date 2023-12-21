using Microsoft.AspNetCore.Mvc;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using System.Threading.Tasks;

namespace SmartCharger.Controllers
{
    [Route("api/")]
    [ApiController]
    public class GoogleLoginController : ControllerBase
    {
        private readonly IGoogleLoginService _googleLoginService;

        public GoogleLoginController(IGoogleLoginService googleLoginService)
        {
            _googleLoginService = googleLoginService;
        }

        [HttpPost("login/google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string accessToken)
        {
           LoginResponseDTO loginResult = await _googleLoginService.LoginWithGoogleAsync(accessToken);

            if (loginResult.Success == true)
            {
                return Ok(loginResult);
            }
            else
            {
                return Unauthorized(loginResult);
            }
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;

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
        public async Task<IActionResult> LoginWithGoogle([FromBody] string authorizationCode)
        {
           LoginResponseDTO loginResult = await _googleLoginService.LoginWithGoogleAsync(authorizationCode);

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

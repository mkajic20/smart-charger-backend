﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;

namespace SmartCharger.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            LoginResponseDTO loginResult = await _loginService.LoginAsync(loginDTO);

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

using Microsoft.EntityFrameworkCore;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Data;
using SmartCharger.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharger.Business.Services
{
    public class LoginService : GenericService<User>, ILoginService
    {
        public LoginService(SmartChargerContext context) : base(context)
        { }
        public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            AuthService authService = new AuthService();

            if (!authService.IsValidEmail(loginDTO.Email))
            {
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = "Login failed.",
                    Error = "Email is not valid."
                };
            }

            if (loginDTO.Password.Length < 6)
            {
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = "Login failed.",
                    Error = "Password must have at least 6 characters."
                };
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (user == null)
            {
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = "Login failed.",
                    Error = "This email is not registered."
                };
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password);

            if (!isPasswordValid)
            {
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = "Login failed.",
                    Error = "Invalid credentials."
                };
            }

            string jwt = authService.GenerateJWT(user);

            if (jwt == null)
            {
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = "Login failed.",
                    Error = "Problem with creating JWT."
                };
            }

            return new LoginResponseDTO
            {
                Success = true,
                Message = "Login successful.",
                User = new UserDTO
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                },
                Token = jwt
            };
        }
    }
}

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
            var response = new LoginResponseDTO { Success = false, Message = "Login failed." };
            AuthService authService = new AuthService();

            if (!authService.IsValidEmail(loginDTO.Email))
            {
                response.Error = "Email is not valid.";
                return response;
            }

            if (loginDTO.Password.Length < 6)
            {
                response.Error = "Password must have at least 6 characters.";
                return response;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (user == null)
            {
                response.Error = "This email is not registered.";
                return response;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password);

            if (!isPasswordValid)
            {
                response.Error = "Invalid credentials.";
                return response;
            }

            string jwt = authService.GenerateJWT(user);

            if (jwt == null)
            {
                response.Error = "Problem with creating JWT.";
                return response;
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

using Microsoft.EntityFrameworkCore;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Data;
using SmartCharger.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartCharger.Business.Services
{
    public class RegisterService : GenericService<User>, IRegisterService
    {
        public RegisterService(SmartChargerContext context) : base(context)
        { }
        public async Task<RegisterResponseDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            if (!IsValidEmail(registerDTO.Email))
            {
                return new RegisterResponseDTO
                {
                    Success = false,
                    Message = "Registration failed.",
                    Error = "Email is not valid."
                };
            }

            if (await _context.Users.AnyAsync(u => u.Email == registerDTO.Email))
            {
                return new RegisterResponseDTO
                {
                    Success = false,
                    Message = "Registration failed.",
                    Error = "Email is already in use."
                };
            }

            if (registerDTO.FirstName.Length < 1 || registerDTO.LastName.Length < 1)
            {
                return new RegisterResponseDTO
                {
                    Success = false,
                    Message = "Registration failed.",
                    Error = "First and last name cannot be empty."
                };
            }

            if (registerDTO.Password.Length < 6)
            {
                return new RegisterResponseDTO
                {
                    Success = false,
                    Message = "Registration failed.",
                    Error = "Password must have at least 6 characters."
                };
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password, salt);

            User user = new User
            {
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                Email = registerDTO.Email,
                Password = hashedPassword,
                Active = true,
                CreationTime = DateTime.Now,
                Salt = salt,
                RoleId = 2
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new RegisterResponseDTO
            {
                Success = true,
                Message = "Registration successful.",
                User = new RegisterDTO
                {
                    FirstName = registerDTO.FirstName,
                    LastName = registerDTO.LastName,
                    Email = registerDTO.Email
                }
            };
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

using Google.Apis.Auth;
using Google.Apis.Oauth2.v2;
using Microsoft.EntityFrameworkCore;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Data;
using SmartCharger.Data.Entities;


namespace SmartCharger.Business.Services
{
    public class GoogleLoginService : IGoogleLoginService 
    {
        private readonly SmartChargerContext _context;
        private readonly IGoogleAuthService _googleAuthService;

        public GoogleLoginService(SmartChargerContext context, IGoogleAuthService googleAuthService)
        {
            _context = context;
            _googleAuthService = googleAuthService;

        }

        public async Task<LoginResponseDTO> LoginWithGoogleAsync(string accessToken)
        {
            AuthService authService = new AuthService();

            var response = await _googleAuthService.GetUserInfoAsync(accessToken);

            if (!response.Success)
            {
                return response;
            }

            var googleUser = response.User;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == googleUser.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = googleUser.Email,
                    FirstName = googleUser.FirstName,
                    LastName = googleUser.LastName,
                    RoleId = 2,
                    CreationTime = DateTime.UtcNow,
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }

            var jwt = authService.GenerateJWT(user);

            if (jwt == null)
            {
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = "Failed to generate JWT."
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


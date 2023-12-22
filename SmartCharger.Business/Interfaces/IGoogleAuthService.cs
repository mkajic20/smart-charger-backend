

using SmartCharger.Business.DTOs;

namespace SmartCharger.Business.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<LoginResponseDTO> GetUserInfoAsync(string accessToken);
    }

}



using SmartCharger.Business.DTOs;

namespace SmartCharger.Business.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<GoogleUserDTO> GetUserInfoAsync(string accessToken);
    }

}

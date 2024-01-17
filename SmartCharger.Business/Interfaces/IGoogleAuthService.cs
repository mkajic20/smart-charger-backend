using SmartCharger.Business.DTOs;

namespace SmartCharger.Business.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<LoginResponseDTO> GetUserInfoFromAuthCodeAsync(string authorizationCode);
    }

}
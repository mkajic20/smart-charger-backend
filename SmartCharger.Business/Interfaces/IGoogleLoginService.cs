using SmartCharger.Business.DTOs;


namespace SmartCharger.Business.Interfaces
{
    public interface IGoogleLoginService
    {
        Task<LoginResponseDTO> LoginWithGoogleAsync(string googleLoginDTO);
    }
}

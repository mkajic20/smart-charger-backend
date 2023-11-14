using SmartCharger.Business.DTOs;

namespace SmartCharger.Business.Interfaces
{
    public interface IChargerService
    {
        Task<ChargerResponseDTO> CreateNewCharger();
        Task<ChargerResponseDTO> GetAllChargers();
        Task<ChargerResponseDTO> UpdateCharger(int chargerId);
        Task<ChargerResponseDTO> DeleteCharger(int chargerId);

    }
}

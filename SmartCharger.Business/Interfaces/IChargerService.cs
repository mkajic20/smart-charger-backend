using SmartCharger.Business.DTOs;

namespace SmartCharger.Business.Interfaces
{
    public interface IChargerService
    {
        Task<ChargerResponseDTO> CreateNewCharger(ChargerDTO charger);
        Task<ChargerResponseDTO> GetAllChargers();
        Task<ChargerResponseDTO> UpdateCharger(int chargerId, ChargerDTO charger);
        Task<ChargerResponseDTO> DeleteCharger(int chargerId);

    }
}

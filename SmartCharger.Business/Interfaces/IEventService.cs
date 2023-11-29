using SmartCharger.Business.DTOs;

namespace SmartCharger.Business.Interfaces
{
    public interface IEventService
    {
        Task<EventResponseDTO> GetUsersChargingHistory(int userId, int page, int pageSize, string search);
        Task<EventResponseDTO> StartCharging(DateTime startTime, int chargerId, int cardId, int userId);
        Task<EventResponseDTO> EndCharging(DateTime endTime, double value);
    }
}

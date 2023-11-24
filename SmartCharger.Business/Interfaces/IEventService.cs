using SmartCharger.Business.DTOs;

namespace SmartCharger.Business.Interfaces
{
    public interface IEventService
    {
        Task<EventResponseDTO> GetUsersChargingHistory(int userId, int page, int pageSize, string search);
    }
}

using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Data;
using SmartCharger.Data.Entities;

namespace SmartCharger.Business.Services
{
    public class EventService : GenericService<Event>, IEventService
    {
        public EventService(SmartChargerContext context) : base(context)
        {
        }

        public Task<EventResponseDTO> GetUsersChargingHistory(int userId, int page = 1, int pageSize = 5, string search = null)
        {
            throw new NotImplementedException();
        }
    }
}
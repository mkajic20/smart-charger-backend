using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Data;
using SmartCharger.Data.Entities;

namespace SmartCharger.Business.Services
{
    public class ChargerService : GenericService<Charger>, IChargerService
    {
        public ChargerService(SmartChargerContext context) : base(context)
        {
        }

        public async Task<ChargerResponseDTO> CreateNewCharger()
        {
            throw new NotImplementedException();
        }

        public  async Task<ChargerResponseDTO> DeleteCharger(int chargerId)
        {
            throw new NotImplementedException();
        }

        public async  Task<ChargerResponseDTO> GetAllChargers()
        {
            throw new NotImplementedException();
        }

        public async Task<ChargerResponseDTO> UpdateCharger(int chargerId)
        {
            throw new NotImplementedException();
        }
    }
}

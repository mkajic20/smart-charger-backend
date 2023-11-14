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

        public async Task<ChargerResponseDTO> CreateNewCharger(ChargerDTO charger)
        {
            if (string.IsNullOrWhiteSpace(charger.Name))
            {
                return new ChargerResponseDTO
                {
                    Success = false,
                    Message = "Charger creation failed.",
                    Error = "Name of the charger cannot be empty."
                };
            }

            if (charger.Latitude < -90 || charger.Latitude > 90)
            {
                return new ChargerResponseDTO
                {
                    Success = false,
                    Message = "Charger creation failed.",
                    Error = "Latitude must be between -90 and 90."
                };
            }
            if (charger.Longitude < -180 || charger.Longitude > 180)
            {
                return new ChargerResponseDTO
                {
                    Success = false,
                    Message = "Charger creation failed.",
                    Error = "Longitude must be between -180 and 180."
                };
            }

            Charger newCharger = new Charger
            {
                Name = charger.Name,
                Latitude = charger.Latitude,
                Longitude = charger.Longitude,
                Active = true,
                CreationTime = DateTime.Now.ToUniversalTime(),
                CreatorId = charger.CreatorId
            };

            _context.Chargers.Add(newCharger);
            await _context.SaveChangesAsync();

            return new ChargerResponseDTO
            {
                Success = true,
                Message = "Charger created successfully."
            };
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

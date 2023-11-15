using Microsoft.EntityFrameworkCore;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Data;
using SmartCharger.Data.Entities;
using System.Runtime.InteropServices;

namespace SmartCharger.Business.Services
{
    public class ChargerService : GenericService<Charger>, IChargerService
    {
        public ChargerService(SmartChargerContext context) : base(context)
        {}
        public async Task<ChargerResponseDTO> CreateNewCharger(ChargerDTO charger)
        {
                     
            if (ValidateName(charger.Name))
            {
                return new ChargerResponseDTO
                {
                    Success = false,
                    Message = "Charger creation failed.",
                    Error = "Name of the charger cannot be empty."
                };
            }

            if (ValidateLatitude(charger.Latitude))
            {
                return new ChargerResponseDTO
                {
                    Success = false,
                    Message = "Charger creation failed.",
                    Error = "Latitude must be between -90 and 90."
                };
            }
            if (ValidateLongitude(charger.Longitude))
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

        private bool ValidateLongitude(double longitude)
        {
            return (longitude < -180 || longitude > 180);
        }

        private bool ValidateLatitude(double latitude)
        {
            return (latitude < -90 || latitude > 90);
        }

        private bool ValidateName(string name)
        {
            return string.IsNullOrWhiteSpace(name);
        }

        public override async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id);
        }
        public async Task<ChargerResponseDTO> DeleteCharger(int chargerId)
        {
            var charger = await _context.Chargers.SingleOrDefaultAsync(c => c.Id == chargerId);
            if (charger == null)
            {
                return new ChargerResponseDTO
                {
                    Success = false,
                    Message = "Unsuccessful deletion of the charger.",
                    Error = $"Charger not found."
                };
            }
           

            await DeleteAsync(chargerId);

            return new ChargerResponseDTO
            {
                Success = true,
                Message = $"Charger deleted successfully."
            };
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

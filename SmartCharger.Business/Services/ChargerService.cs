using Microsoft.EntityFrameworkCore;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Data;
using SmartCharger.Data.Entities;
using System;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace SmartCharger.Business.Services
{
    public class ChargerService : GenericService<Charger>, IChargerService
    {
        public ChargerService(SmartChargerContext context) : base(context)
        { }
        public async Task<ChargerResponseDTO> CreateNewCharger(ChargerDTO charger)
        {
            try
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
                    Active = false,
                    CreationTime = DateTime.Now.ToUniversalTime(),
                    CreatorId = charger.CreatorId
                };

                _context.Chargers.Add(newCharger);
                await _context.SaveChangesAsync();

                ChargerDTO chargerDTO = MapChargerToDTO(newCharger);



                return new ChargerResponseDTO
                {
                    Success = true,
                    Message = "Charger created successfully.",
                    Charger = chargerDTO
                };
            }
            catch (Exception ex)
            {
                return new ChargerResponseDTO
                {
                    Success = false,
                    Message = "Charger creation failed.",
                    Error = ex.Message
                };
            }
        }

        private ChargerDTO MapChargerToDTO(Charger charger)
        {
            return new ChargerDTO
            {
                Id = charger.Id,
                Name = charger.Name,
                Latitude = charger.Latitude,
                Longitude = charger.Longitude,
                Active = charger.Active,
                CreationTime = charger.CreationTime,
                LastSync = charger.LastSync,
                CreatorId = charger.CreatorId

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


        public async Task<ChargerResponseDTO> DeleteCharger(int chargerId)
        {
            try
            {
                var charger = await _context.Chargers.SingleOrDefaultAsync(c => c.Id == chargerId);
                if (charger == null)
                {
                    return new ChargerResponseDTO
                    {
                        Success = false,
                        Message = "Unsuccessful deletion of the charger.",
                        Error = "Charger not found.",
                        Charger = null
                    };
                }



                _context.Chargers.Remove(charger);
                await _context.SaveChangesAsync();

                return new ChargerResponseDTO
                {
                    Success = true,
                    Message = "Charger deleted successfully.",
                    Charger = null

                };
            }
            catch (Exception ex)
            {
                return new ChargerResponseDTO
                {
                    Success = false,
                    Message = "Unsuccessful deletion of the charger.",
                    Error = ex.Message,
                    Charger = null
                };
            }
        }

        public async Task<ChargerResponseDTO> GetAllChargers(int page = 1, int pageSize = 20, string search = null)
        {
            try
            {
                IQueryable<Charger> query = _context.Chargers;

                if (!string.IsNullOrEmpty(search))
                {
                    string searchLower = search.ToLower();

                    query = query.Where(c => c.Name.ToLower().Contains(search));

                }

                var totalItems = await query.CountAsync();

                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                if (totalItems == 0 || page > totalPages)
                {
                    return new ChargerResponseDTO
                    {
                        Success = false,
                        Message = "There are no chargers with that parameters.",
                        Chargers = null
                    };
                }

                var chargers = await query
                    .OrderBy(c => c.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new ChargerDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Latitude = c.Latitude,
                        Longitude = c.Longitude,
                        Active = c.Active,
                        CreationTime = c.CreationTime,
                        LastSync = c.LastSync,
                        CreatorId = c.CreatorId
                    }).ToListAsync();


                return new ChargerResponseDTO
                {
                    Success = true,
                    Message = "List of chargers.",
                    Chargers = chargers,
                    Page = page,
                    TotalPages = totalPages
                };
            }
            catch (Exception ex)
            {
                return new ChargerResponseDTO
                {
                    Success = false,
                    Message = "An error occurred.",
                    Error = ex.Message,
                    Chargers = null
                };
            }
        }


        public async Task<ChargerResponseDTO> UpdateCharger(int chargerId, ChargerDTO charger)
        {
            try
            {
                var chargerEntity = await _context.Chargers.SingleOrDefaultAsync(c => c.Id == chargerId);
                if (chargerEntity == null)
                {
                    return new ChargerResponseDTO
                    {
                        Success = false,
                        Message = "Unsuccessful update of the charger. ",
                        Error = "Charger not found."
                    };
                }

                if (ValidateName(charger.Name))
                {
                    return new ChargerResponseDTO
                    {
                        Success = false,
                        Message = "Charger update failed.",
                        Error = "Name of the charger cannot be empty."
                    };
                }

                if (ValidateLatitude(charger.Latitude))
                {
                    return new ChargerResponseDTO
                    {
                        Success = false,
                        Message = "Charger update failed.",
                        Error = "Latitude must be between -90 and 90."
                    };
                }
                if (ValidateLongitude(charger.Longitude))
                {
                    return new ChargerResponseDTO
                    {
                        Success = false,
                        Message = "Charger update failed.",
                        Error = "Longitude must be between -180 and 180."
                    };
                }

                UpdateCharger(chargerEntity, charger);

                await _context.SaveChangesAsync();

                ChargerDTO chargerDTO = MapChargerToDTO(chargerEntity);

                return new ChargerResponseDTO
                {
                    Success = true,
                    Message = "Charger updated successfully.",
                    Charger = chargerDTO
                };
            }
            catch (Exception ex)
            {
                return new ChargerResponseDTO
                {
                    Success = false,
                    Message = "Unsuccessful update of the charger. ",
                    Error = ex.Message
                };
            }
        }

        private void UpdateCharger(Charger chargerEntity, ChargerDTO charger)
        {
            chargerEntity.Name = charger.Name;
            chargerEntity.Latitude = charger.Latitude;
            chargerEntity.Longitude = charger.Longitude;
        }
    }
}

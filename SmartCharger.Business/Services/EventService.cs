using Microsoft.EntityFrameworkCore;
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

        public async Task<EventResponseDTO> GetUsersChargingHistory(int userId, int page = 1, int pageSize = 5, string search = null)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(c => c.Id == userId);
                if (user == null)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "User not found.",
                        Error = "User not found."
                    };
                }
                IQueryable<Event> query = _context.Events.Where(e => e.UserId == userId && e.EndTime != null);


                if (!string.IsNullOrEmpty(search))
                {
                    string searchLower = search.ToLower();

                    query = query.Where(e => e.Card.Name.ToLower().Contains(searchLower)
                                           || e.Charger.Name.ToLower().Contains(searchLower));
                }

                var totalItems = await query.CountAsync();

                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                if (totalItems == 0 || page > totalPages)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "There are no events with that parameters.",
                        Events = null
                    };
                }


                var events = await query
                  .OrderBy(e => e.Id)
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .Select(e => new EventDTO
                  {
                      Id = e.Id,
                      StartTime = e.StartTime,
                      EndTime = e.EndTime,
                      Volume = e.Volume,
                      Card = new CardDTO
                      {
                          Name = e.Card.Name,
                          Value = e.Card.Value,
                          Active = e.Card.Active,
                          User = null,
                      },
                      Charger = new ChargerDTO
                      {
                          Id = e.Charger.Id,
                          Name = e.Charger.Name,
                          Latitude = e.Charger.Latitude,
                          Longitude = e.Charger.Longitude,
                          CreationTime = e.Charger.CreationTime,
                          Active = e.Charger.Active,
                          CreatorId = e.Charger.CreatorId
                      },
                      User = new UserDTO
                      {
                          FirstName = e.User.FirstName,
                          LastName = e.User.LastName,
                          Email = e.User.Email
                      }
                  }).ToListAsync();

                return new EventResponseDTO
                {
                    Success = true,
                    Message = $"List of {user.FirstName} {user.LastName}'s events.",
                    Events = events,
                    Page = page,
                    TotalPages = totalPages

                };
            }
            catch (Exception ex)
            {
                return new EventResponseDTO
                {
                    Success = false,
                    Message = "An error occurred: " + ex.Message + ".",
                    Events = null
                };
            }
        }

        public async Task<EventResponseDTO> StartCharging(EventChargingDTO eventDTO)
        {
            try
            {
                var charger = await _context.Chargers.SingleOrDefaultAsync(c => c.Id == eventDTO.ChargerId);
                if (charger == null)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "Charger with ID: " + eventDTO.ChargerId + " not found."
                    };
                }
                if (charger.Active == true)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "Charger is already in use."
                    };
                }
                var card = await _context.Cards
                    .Include(c => c.User)
                    .SingleOrDefaultAsync(c => c.Id == eventDTO.CardId);
                if (card == null)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "Card with ID: " + eventDTO.CardId + " not found."
                    };
                }
                if (card.Active == false)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "RFID card is not active."
                    };
                }
                if (card.UsageStatus == true)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "RFID card is already in use."
                    };
                }
                if (card.User.Active == false)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "User is not active."
                    };
                }

                Event newEvent = new Event
                {
                    StartTime = eventDTO.StartTime,
                    ChargerId = eventDTO.ChargerId,
                    CardId = eventDTO.CardId,
                    UserId = eventDTO.UserId
                };

                _context.Events.Add(newEvent);
                charger.Active = true;
                card.UsageStatus = true;

                await _context.SaveChangesAsync();

                return new EventResponseDTO
                {
                    Success = true,
                    Message = "Charging started.",
                    Event = new EventChargingDTO
                    {
                        Id = newEvent.Id,
                        StartTime = newEvent.StartTime,
                        ChargerId = newEvent.ChargerId,
                        CardId = newEvent.CardId,
                        UserId = newEvent.UserId
                    }
                };
            }
            catch (Exception ex)
            {
                return new EventResponseDTO
                {
                    Success = false,
                    Message = "An error occurred.",
                    Error = ex.Message
                };
            }
        }

        public async Task<EventResponseDTO> EndCharging(EventChargingDTO eventDTO)
        {
            try
            {
                var chargingEvent = await _context.Events
                    .Include(e => e.Charger)
                    .Include(e => e.Card)
                    .Include(e => e.User)
                    .SingleOrDefaultAsync(e => e.Id == eventDTO.Id);

                if (chargingEvent == null)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "Event with ID: " + eventDTO.Id + " not found."
                    };
                }

                if (chargingEvent.EndTime != null)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "Charging has already ended."
                    };
                }

                chargingEvent.EndTime = eventDTO.EndTime;
                chargingEvent.Volume = eventDTO.Volume;

                chargingEvent.Charger.Active = false;
                chargingEvent.Card.UsageStatus = false;

                Event selectedEvent = new Event
                {
                    Id = chargingEvent.Id,
                    StartTime = chargingEvent.StartTime,
                    EndTime = chargingEvent.EndTime,
                    Volume = chargingEvent.Volume,
                    Charger = chargingEvent.Charger,
                    Card = chargingEvent.Card,
                    User = chargingEvent.User
                };

                await _context.SaveChangesAsync();

                return new EventResponseDTO
                {
                    Success = true,
                    Message = "Charging has ended.",
                    Event = new EventChargingDTO
                    {
                        Id = selectedEvent.Id,
                        StartTime = selectedEvent.StartTime,
                        EndTime = selectedEvent.EndTime,
                        Volume = selectedEvent.Volume,
                        ChargerId = selectedEvent.Charger.Id,
                        CardId = selectedEvent.Card.Id,
                        UserId = selectedEvent.User.Id
                    }
                };
            }
            catch (Exception ex)
            {
                return new EventResponseDTO
                {
                    Success = false,
                    Message = "An error occurred.",
                    Error = ex.Message
                };
            }
        }
    }
}
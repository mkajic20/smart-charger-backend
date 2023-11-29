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
                    Events = events
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

        public async Task<EventResponseDTO> StartCharging(DateTime startTime, int chargerId, int cardId, int userId)
        {
            try
            {
                var charger = await _context.Chargers.SingleOrDefaultAsync(c => c.Id == chargerId);
                if (charger == null)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "Charger with ID: " + charger.Id + " not found."
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
                var card = await _context.Cards.SingleOrDefaultAsync(c => c.Id == cardId);
                if (card == null)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "Card with ID: " + card.Id + " not found."
                    };
                }
                if (card.Active == false)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "RFID Card is not active."
                    };
                }
                if (card.UsageStatus == true)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "RFID Card is already in use."
                    };
                }
                var user = await _context.Users.SingleOrDefaultAsync(c => c.Id == userId);
                if (user.Active == false)
                {
                    return new EventResponseDTO
                    {
                        Success = false,
                        Message = "User is not active."
                    };
                }

                var newEvent = new Event
                {
                    StartTime = startTime,
                    ChargerId = chargerId,
                    CardId = cardId,
                    UserId = userId
                };

                _context.Events.Add(newEvent);
                charger.Active = true;
                card.UsageStatus = true;

                await _context.SaveChangesAsync();

                return new EventResponseDTO
                {
                    Success = true,
                    Message = "Charging started.",
                    Event = new EventDTO
                    {
                        Id = newEvent.Id,
                        StartTime = newEvent.StartTime,
                        EndTime = newEvent.EndTime,
                        Volume = newEvent.Volume,
                        Card = new CardDTO
                        {
                            Name = card.Name,
                            Value = card.Value,
                            Active = card.Active
                        },
                        Charger = new ChargerDTO
                        {
                            Id = charger.Id,
                            Name = charger.Name,
                            Latitude = charger.Latitude,
                            Longitude = charger.Longitude,
                            CreationTime = charger.CreationTime,
                            Active = charger.Active,
                        },
                        User = new UserDTO
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email
                        }
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

        public async Task<EventResponseDTO> EndCharging(DateTime endTime, double value)
        {
            throw new NotImplementedException();
        }
    }
}
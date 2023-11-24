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
                IQueryable<Event> query = _context.Events.Where(e => e.UserId == userId);


                if (!string.IsNullOrEmpty(search))
                {
                    string searchLower = search.ToLower();

                    query = query.Where(e => e.Card.Name.ToLower().Contains(searchLower)
                                           || e.Charger.Name.ToLower().Contains(searchLower)
                                           || e.User.FirstName.ToLower().Contains(searchLower)
                                           || e.User.LastName.ToLower().Contains(searchLower));
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
    }
}
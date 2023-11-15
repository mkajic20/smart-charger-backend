using Microsoft.EntityFrameworkCore;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Data;
using SmartCharger.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharger.Business.Services
{
    public class CardService : GenericService<Card>, ICardService
    {
        public CardService(SmartChargerContext context) : base(context)
        { }
        public async Task<CardsResponseDTO> GetAllCards(int page = 1, int pageSize = 20, string search = null)
        {
            try
            {
                IQueryable<Card> query = _context.Cards;

                if (!string.IsNullOrEmpty(search))
                {
                    string searchLower = search.ToLower();

                    query = query.Where(c =>
                        c.User.FirstName.ToLower().Contains(search) ||
                        c.User.LastName.ToLower().Contains(search) ||
                        c.Name.ToLower().Contains(search) ||
                        c.Value.ToLower().Contains(search)
                    );
                }

                var totalItems = await query.CountAsync();

                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                if (totalItems == 0 || page > totalPages)
                {
                    return new CardsResponseDTO
                    {
                        Success = false,
                        Message = "There are no RFID cards with that parameters.",
                        Cards = null
                    };
                }

                var cards = await query
                    .OrderBy(c => c.Id)
                    .Include(c => c.User)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new CardDTO
                    {
                        Id = c.Id,
                        Value = c.Value,
                        Active = c.Active,
                        Name = c.Name,
                        User = new UserDTO
                        {
                            Id = c.User.Id,
                            FirstName = c.User.FirstName,
                            LastName = c.User.LastName,
                            Email = c.User.Email,
                        }
                    }).ToListAsync();

                return new CardsResponseDTO
                {
                    Success = true,
                    Message = "List of RFID cards with users.",
                    Cards = cards,
                    Page = page,
                    TotalPages = totalPages
                };
            }
            catch (Exception ex)
            {
                return new CardsResponseDTO
                {
                    Success = false,
                    Message = "An error occurred.",
                    Error = ex.Message,
                    Cards = null
                };
            }
        }
        public async Task<CardsResponseDTO> GetCardById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CardsResponseDTO> DeleteCard(int cardId)
        {
            throw new NotImplementedException();
        }

        public async Task<CardsResponseDTO> UpdateActiveStatus(int cardId)
        {
            throw new NotImplementedException();
        }
    }
}

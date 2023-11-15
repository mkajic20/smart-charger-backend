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
        public async Task<CardsResponseDTO> GetAllCards()
        {
            try
            {
                var cards = await _context.Cards
                  .OrderBy(c => c.Id)
                  .Include(c => c.User)
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
                    Message = "List of RFID cards with users",
                    Cards = cards
                };
            }
            catch (Exception ex)
            {
                return new CardsResponseDTO
                {
                    Success = false,
                    Message = "An error occurred: " + ex.Message + ".",
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

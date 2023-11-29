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
        public async Task<CardsResponseDTO> GetCardById(int cardId)
        {
            try
            {
                var card = await _context.Cards
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == cardId);

                if (card == null)
                {
                    return new CardsResponseDTO
                    {
                        Success = false,
                        Message = "There is no RFID card with that ID.",
                        Card = null
                    };
                }

                return new CardsResponseDTO
                {
                    Success = true,
                    Message = "RFID card with user.",
                    Card = MapCardToDTO(card)
                };
            }
            catch (Exception ex)
            {
                return new CardsResponseDTO
                {
                    Success = false,
                    Message = "An error occurred.",
                    Error = ex.Message,
                    Card = null
                };
            }
        }
        public async Task<CardsResponseDTO> UpdateActiveStatus(int cardId)
        {
            try
            {
                var card = await _context.Cards
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == cardId);

                if (card == null)
                {
                    return new CardsResponseDTO
                    {
                        Success = false,
                        Message = "There is no RFID card with that ID.",
                        Card = null
                    };
                }

                card.Active = !card.Active;

                _context.Cards.Update(card);
                await _context.SaveChangesAsync();

                return new CardsResponseDTO
                {
                    Success = true,
                    Message = "RFID card with ID:" + cardId + " updated to " + card.Active + ".",
                    Card = MapCardToDTO(card)
                };
            }
            catch (Exception ex)
            {
                return new CardsResponseDTO
                {
                    Success = false,
                    Message = "An error occurred.",
                    Error = ex.Message,
                    Card = null
                };
            }
        }
        public async Task<CardsResponseDTO> DeleteCard(int cardId)
        {
            try
            {
                var card = await _context.Cards
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == cardId);

                if (card == null)
                {
                    return new CardsResponseDTO
                    {
                        Success = false,
                        Message = "There is no RFID card with that ID.",
                        Card = null
                    };
                }

                _context.Cards.Remove(card);
                await _context.SaveChangesAsync();

                return new CardsResponseDTO
                {
                    Success = true,
                    Message = "RFID card with ID:" + cardId + " is deleted.",
                    Card = null
                };
            }
            catch (Exception ex)
            {
                return new CardsResponseDTO
                {
                    Success = false,
                    Message = "An error occurred.",
                    Error = ex.Message,
                    Card = null
                };
            }
        }

        public async Task<CardsResponseDTO> GetAllCardsForUser(int userId)
        {
            try
            {
                var cards = await _context.Cards
                    .Where(c => c.UserId == userId)
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

                if (cards.Count == 0)
                {
                    return new CardsResponseDTO
                    {
                        Success = false,
                        Message = "User with ID:" + userId + " has no RFID card.",
                        Cards = null
                    };
                }
                else
                {
                    return new CardsResponseDTO
                    {
                        Success = true,
                        Message = "List of RFID cards for user with ID:" + userId + ".",
                        Cards = cards,
                    };
                }
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

        public async Task<CardsResponseDTO> GetCardByIdForUser(int cardId, int userId)
        {
            try
            {
                var card = await _context.Cards
                    .Where(c => c.UserId == userId)
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == cardId);

                if (card == null)
                {
                    return new CardsResponseDTO
                    {
                        Success = false,
                        Message = "RFID card with ID:" + cardId + " doesn't exist.",
                        Card = null
                    };
                }
                else
                {
                    return new CardsResponseDTO
                    {
                        Success = true,
                        Message = "RFID card.",
                        Card = MapCardToDTO(card),
                    };
                }
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

        public async Task<CardsResponseDTO> AddCard(AddCardDTO card, int userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return new CardsResponseDTO
                    {
                        Success = false,
                        Message = "User with ID:" + userId + " doesn't exist.",
                        Card = null
                    };
                }

                if (card.Value == null)
                {
                    return new CardsResponseDTO
                    {
                        Success = false,
                        Message = "RFID card value is required.",
                        Card = null
                    };
                }

                if (card.Name == null)
                {
                    return new CardsResponseDTO
                    {
                        Success = false,
                        Message = "RFID card name is required.",
                        Card = null
                    };
                }

                Card newCard = new Card
                {
                    Value = card.Value,
                    Active = true,
                    Name = card.Name,
                    UserId = user.Id
                };

                var cardExists = await _context.Cards.AnyAsync(c => c.Value == card.Value);
                if (cardExists)
                {
                    return new CardsResponseDTO
                    {
                        Success = false,
                        Message = "RFID card with same value already exists.",
                        Card = null
                    };
                }

                var cardsCount = await _context.Cards.Where(c => c.UserId == user.Id).CountAsync();
                if (cardsCount >= 5)
                {
                    return new CardsResponseDTO
                    {
                        Success = false,
                        Message = "User with ID:" + user.Id + " reached limit of 5 RFID cards.",
                        Card = null
                    };
                }

                await _context.Cards.AddAsync(newCard);
                await _context.SaveChangesAsync();

                return new CardsResponseDTO
                {
                    Success = true,
                    Message = "Successfully added RFID card.",
                    Card = MapCardToDTO(newCard)
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

        public async Task<CardsResponseDTO> DeleteCardForUser(int cardId, int userId)
        {
            try
            {
                var card = await _context.Cards
                    .Where(c => c.UserId == userId)
                    .FirstOrDefaultAsync(c => c.Id == cardId);

                if (card == null)
                {
                    return new CardsResponseDTO
                    {
                        Success = false,
                        Message = "RFID card with ID:" + cardId + " doesn't exist.",
                        Card = null
                    };
                }

                _context.Cards.Remove(card);
                await _context.SaveChangesAsync();

                return new CardsResponseDTO
                {
                    Success = true,
                    Message = "Successfully deleted RFID card with ID:" + cardId + ".",
                    Card = null
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

        public async Task<ResponseBaseDTO> VerifyCard(int cardId)
        {
            throw new NotImplementedException();
        }

        private CardDTO MapCardToDTO(Card card)
        {
            return new CardDTO
            {
                Id = card.Id,
                Value = card.Value,
                Active = card.Active,
                Name = card.Name,
                User = new UserDTO
                {
                    Id = card.User.Id,
                    FirstName = card.User.FirstName,
                    LastName = card.User.LastName,
                    Email = card.User.Email,
                }
            };
        }
    }
}

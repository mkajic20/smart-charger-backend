using SmartCharger.Business.DTOs;

namespace SmartCharger.Business.Interfaces
{
    public interface ICardService
    {
        // Admin
        Task<CardsResponseDTO> GetAllCards(int page, int pageSize, string search);
        Task<CardsResponseDTO> GetCardById(int cardId);
        Task<CardsResponseDTO> UpdateActiveStatus(int cardId);
        Task<CardsResponseDTO> DeleteCard(int cardId);
        // Customer
        Task<CardsResponseDTO> GetAllCardsForUser(int userId);
        Task<CardsResponseDTO> GetCardByIdForUser(int cardId, int userId);
        Task<CardsResponseDTO> AddCard(AddCardDTO card, int userId);
        Task<CardsResponseDTO> DeleteCardForUser(int cardId, int userId);
    }
}

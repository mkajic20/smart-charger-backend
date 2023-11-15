using SmartCharger.Business.DTOs;

namespace SmartCharger.Business.Interfaces
{
    public interface ICardService
    {
        Task<CardsResponseDTO> GetAllCards(int page, int pageSize);
        Task<CardsResponseDTO> GetCardById(int cardId);
        Task<CardsResponseDTO> UpdateActiveStatus(int cardId);
        Task<CardsResponseDTO> DeleteCard(int cardId);
    }
}

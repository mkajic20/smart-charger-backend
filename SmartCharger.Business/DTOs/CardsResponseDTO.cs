using System.Text.Json.Serialization;

namespace SmartCharger.Business.DTOs
{
    public class CardsResponseDTO
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<CardDTO>? Cards { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CardDTO? Card { get; set; }
    }
}

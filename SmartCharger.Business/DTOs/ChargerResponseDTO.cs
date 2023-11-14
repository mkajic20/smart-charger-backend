
using System.Text.Json.Serialization;

namespace SmartCharger.Business.DTOs
{
    public class ChargerResponseDTO : ResponseBaseDTO
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ChargerDTO>? Chargers { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ChargerDTO? Charger { get; set; }
    }
}

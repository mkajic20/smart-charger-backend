using System.Text.Json.Serialization;

namespace SmartCharger.Business.DTOs
{
    public class EventResponseDTO : ResponseBaseDTO
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<EventDTO>? Events { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EventDTO? Event { get; set; }
    }
}

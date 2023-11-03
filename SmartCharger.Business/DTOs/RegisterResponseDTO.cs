using System.Text.Json.Serialization;

namespace SmartCharger.Business.DTOs
{
    public class RegisterResponseDTO : ResponseBaseDTO
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RegisterDTO? User { get; set; }
    }
}

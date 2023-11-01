using System.Text.Json.Serialization;

namespace SmartCharger.Business.DTOs
{
    public class RegisterResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Error { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RegisterDTO? User { get; set; }
    }
}

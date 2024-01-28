using System.Text.Json.Serialization;

namespace SmartCharger.Business.DTOs
{
    public class SingleUserResponseDTO : ResponseBaseDTO
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public UserDTO? User { get; set; }
    }
}

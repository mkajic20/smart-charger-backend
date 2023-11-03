using System.Text.Json.Serialization;

namespace SmartCharger.Business.DTOs
{
    public class UsersResponseDTO : ResponseBaseDTO
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<UserDTO>? Users { get; set; }
    }
}



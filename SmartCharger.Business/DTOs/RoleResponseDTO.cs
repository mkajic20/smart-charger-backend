
using System.Text.Json.Serialization;

namespace SmartCharger.Business.DTOs
{
    public class RoleResponseDTO : ResponseBaseDTO
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<RoleDTO> Roles { get; set; }
    }
}

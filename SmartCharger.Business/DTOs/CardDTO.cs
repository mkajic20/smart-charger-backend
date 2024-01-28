using System.Text.Json.Serialization;

namespace SmartCharger.Business.DTOs
{
    public class CardDTO : BaseDTO
    {
        public string Value { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public UserDTO User { get; set; }
    }
}

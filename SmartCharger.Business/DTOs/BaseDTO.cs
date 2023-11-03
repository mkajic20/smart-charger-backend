using System.Text.Json.Serialization;

namespace SmartCharger.Business.DTOs
{
    public class BaseDTO
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Id { get; set; }
    }
}
using System.Text.Json.Serialization;

namespace SmartCharger.Business.DTOs
{
    public class ChargerDTO : BaseDTO
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreationTime { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? LastSync { get; set; }
        public bool Active { get; set; }
        public int CreatorId { get; set; }
    }
}

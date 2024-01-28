namespace SmartCharger.Business.DTOs
{
    public class EventDTO : BaseDTO
    {
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? Volume { get; set; }
        public ChargerDTO Charger { get; set; }
        public CardDTO Card { get; set; }
        public UserDTO User { get; set; }
    }
}

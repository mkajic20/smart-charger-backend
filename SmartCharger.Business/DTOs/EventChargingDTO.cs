namespace SmartCharger.Business.DTOs
{
    public class EventChargingDTO : BaseDTO
    {
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? Volume { get; set; }
        public int ChargerId { get; set; }
        public int CardId { get; set; }
        public int UserId { get; set; }
    }
}

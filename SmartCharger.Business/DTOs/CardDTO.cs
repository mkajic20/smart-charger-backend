namespace SmartCharger.Business.DTOs
{
    public class CardDTO : BaseDTO
    {
        public string Value { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public UserDTO User { get; set; }
    }
}

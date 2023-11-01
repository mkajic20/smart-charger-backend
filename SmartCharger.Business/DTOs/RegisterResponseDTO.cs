namespace SmartCharger.Business.DTOs
{
    public class RegisterResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? Error { get; set; }
        public RegisterDTO? User { get; set; }
    }
}

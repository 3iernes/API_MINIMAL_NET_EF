namespace API_ESP_GW.Clas
{
    public class ErrorResponse
    {
        public required bool Success { get; set; } = false;
        public required string ErrorMessage { get; set; }
    }
}

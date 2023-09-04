namespace API_ESP_GW.Clas.Responses
{
    public class ErrorResponse
    {
        public required bool Success { get; set; } = false;
        public required string ErrorMessage { get; set; }
    }
}

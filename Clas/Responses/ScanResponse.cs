namespace API_ESP_GW.Clas.Responses
{
    public class ScanResponse
    {
        public required bool Encontrado { get; set; } = false;
        public required bool Creado { get; set; } = false;
        public required bool Error { get; set; } = false;
        public string? Mensaje { get; set; } = "";
    }
}

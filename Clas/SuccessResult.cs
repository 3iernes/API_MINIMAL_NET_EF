namespace API_ESP_GW.Clas
{
    public class SuccessResult
    {
        public required bool Succes { get; set; } = false;
        public string? Message { get; set; }
        public bool? Find { get; set; } = false;
    }
}

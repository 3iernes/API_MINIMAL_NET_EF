namespace API_ESP_GW.Models
{
    public class BarCode
    {
        public int Id { get; set; }
        public required string CodeNumbers { get; set; }
        public required bool Active { get; set; }
        public int Pos { get; set; }
    }
}

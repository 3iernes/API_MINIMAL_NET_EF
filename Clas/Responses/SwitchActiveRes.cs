﻿namespace API_ESP_GW.Clas.Responses
{
    public class SwitchActiveRes
    {
        public required bool Encontrado { get; set; } = false;
        public required bool Error { get; set; } = false;
        public required bool Activo { get; set; } = false;
        public string? Mensaje { get; set; } = "";
    }
}

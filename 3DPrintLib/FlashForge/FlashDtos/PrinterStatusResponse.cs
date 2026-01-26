using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    public class PrinterStatusResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("temperature")]
        public TemperatureInfo Temperature { get; set; } = default!;

        [JsonPropertyName("print_progress")]
        public int PrintProgress { get; set; }

        [JsonPropertyName("current_file")]
        public string CurrentFile { get; set; } = string.Empty;
    }
}

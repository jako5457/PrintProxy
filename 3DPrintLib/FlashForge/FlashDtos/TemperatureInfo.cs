using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    public class TemperatureInfo
    {
        [JsonPropertyName("nozzle")]
        public double Nozzle { get; set; }

        [JsonPropertyName("bed")]
        public double Bed { get; set; }

        [JsonPropertyName("chamber")]
        public double Chamber { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    internal class FlashForgePrintRequest : FlashforgeAuthRequest
    {
        [JsonPropertyName("fileName")]
        public string FileName { get; set; } = string.Empty;

        [JsonPropertyName("levelingBeforePrint")]
        public bool LevelingBeforePrint { get; set; } = false;
    }
}

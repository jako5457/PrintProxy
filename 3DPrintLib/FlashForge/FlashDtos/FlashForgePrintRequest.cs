using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    internal class FlashForgePrintRequest : FlashforgeAuthRequest
    {
        [JsonProperty("fileName")]
        public string FileName { get; set; } = string.Empty;

        [JsonProperty("levelingBeforePrint")]
        public bool LevelingBeforePrint { get; set; } = false;
    }
}

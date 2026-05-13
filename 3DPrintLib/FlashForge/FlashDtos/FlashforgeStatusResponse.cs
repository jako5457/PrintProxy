using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    public class FlashforgeStatusResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("Message")]
        public string message { get; set; } = string.Empty;
    }
}

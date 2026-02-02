using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    public class FlashforgeAuthRequest
    {
        [JsonPropertyName("serialNumber")]
        [JsonPropertyOrder(1)]
        public string SerialNumber { get; set; } = string.Empty;

        [JsonPropertyName("checkCode")]
        [JsonPropertyOrder(2)]
        public string CheckCode { get; set; } = string.Empty;
    }
}

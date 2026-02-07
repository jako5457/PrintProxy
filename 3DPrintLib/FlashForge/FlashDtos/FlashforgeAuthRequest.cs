using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    public class FlashforgeAuthRequest
    {
        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; } = string.Empty;

        [JsonProperty("checkCode")]
        public string CheckCode { get; set; } = string.Empty;
    }
}

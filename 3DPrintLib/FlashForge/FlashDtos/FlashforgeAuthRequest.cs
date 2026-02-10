using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    public class FlashforgeAuthRequest
    {
        [JsonProperty("serialNumber",Order = 1)]
        public string SerialNumber { get; set; } = string.Empty;

        [JsonProperty("checkCode",Order = 2)]
        public string CheckCode { get; set; } = string.Empty;
    }
}

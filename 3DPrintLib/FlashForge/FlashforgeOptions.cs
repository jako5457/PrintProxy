using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintLib.FlashForge
{
    public class FlashforgeOptions
    {

        [JsonProperty("Printer_ip")]
        public required string PrinterIP { get; set; }

        public int Port { get; set; } = 8898;

        [JsonProperty("Serialnumber")]
        public required string SerialNumber { get; set; }

        [JsonProperty("check_code")]
        public required string CheckCode { get; set; }

        [JsonIgnore]
        public string FullAddress { get => $"http://{PrinterIP}:{Port}"; }

    }
}

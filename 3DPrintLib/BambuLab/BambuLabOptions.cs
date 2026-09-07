using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3DPrintLib.BambuLab
{
    public class BambuLabOptions
    {

        [JsonProperty("Printer_ip")]
        public string PrinterIP { get; set; } = string.Empty;

        [JsonProperty("Access_code")]
        public string AccessCode { get; set; } = string.Empty;

        [JsonProperty("Serial_number")]
        public string SerialNunber { get; set; } = string.Empty;
        
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace _3DPrintLib.BambuLab
{
    public class BambuLabOptions
    {
        [JsonProperty("Printer_name")]
        public string PrinterName { get; set; } = string.Empty;


        [JsonProperty("Printer_ip")]
        public string PrinterIP { get; set; } = string.Empty;

        [JsonProperty("Access_code")]
        public string AccessCode { get; set; } = string.Empty;

        [JsonProperty("Serial_number")]
        public string SerialNunber { get; set; } = string.Empty;

        [JsonIgnore]
        public string Identifier
        {
            get
            {
                using SHA256 sha = SHA256.Create();

                byte[] data = Encoding.UTF8.GetBytes($"{SerialNunber}:{PrinterIP}");

                byte[] identifier = sha.ComputeHash(data);

                return Convert.ToBase64String(identifier);
            }
        }
    }
}

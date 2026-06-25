using Newtonsoft.Json;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

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

        [JsonIgnore]
        public string Identifier { 
          get
          {
                using SHA256 sha = SHA256.Create();

                byte[] data = Encoding.UTF8.GetBytes($"{SerialNumber}:{PrinterIP}:{Port}");

                byte[] identifier = sha.ComputeHash(data);

                return Convert.ToBase64String(identifier);
          } 
        }

    }
}

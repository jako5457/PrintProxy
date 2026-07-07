using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PrintLib.Moonraker
{
    public class MoonrakerOptions
    {
        [JsonProperty("printer_name")]
        public string PrinterName { get; set; }

        [JsonProperty("endpoint")]
        public string Endpoint { get; set; }

        [JsonIgnore]
        public string Identifier
        {
            get
            {
                using SHA256 sha = SHA256.Create();

                byte[] data = Encoding.UTF8.GetBytes($"{Endpoint}:{PrinterName}");

                byte[] identifier = sha.ComputeHash(data);

                return Convert.ToBase64String(identifier);
            }
        }
    }
}

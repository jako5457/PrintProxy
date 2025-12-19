using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.OctoPrint.OctoDtos
{
    internal class OctoPrintVersionInfo
    {
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;

        [JsonPropertyName("safemode")]
        public string Safemode { get; set; } = string.Empty;
    }
}

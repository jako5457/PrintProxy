using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PrintLib.OctoPrint.OctoDtos
{
    internal class OctoPrintConnectionInfo
    {
        [JsonPropertyName("current")]
        public CurrentConnectionInfo Current { get; set; } = default!;

        [JsonPropertyName("options")]
        public ConnectionOptions Options { get; set; } = default!;
    }

    internal class CurrentConnectionInfo
    {
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("printerProfile")]
        public string PrinterProfile { get; set; } = string.Empty;
    }

    internal class ConnectionOptions
    {
        [JsonPropertyName("printerProfiles")]
        public PrinterProfile[] PrinterProfiles { get; set; } = default!;

        [JsonPropertyName("printerProfilePreference")]
        public string PrinterProfilePreference { get; set; } = string.Empty;
    }

    internal class PrinterProfile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.OctoPrint.OctoDtos
{
    internal class OctoPrintJobInfo
    {
        [JsonPropertyName("state")]
        public string State { get; set; } = "N/A";

        [JsonPropertyName("job")]
        public OctoPrinterJob Job { get; set; } = default!;

        [JsonPropertyName("progress")]
        public OctoPrinterProgress Progress { get; set; } = default!;

    }

    internal class OctoPrinterJob
    {
        [JsonPropertyName("file")]
        public OctoPrinterJobFile File { get; set; } = default!;
    }

    internal class OctoPrinterJobFile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "N/A";
    }

    internal class OctoPrinterProgress
    {
        [JsonPropertyName("completion")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Completion { get; set; } = 0;

        [JsonPropertyName("printTime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? PrintTime { get; set; } = 0;

        [JsonPropertyName("printTimeLeft")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? PrintTimeLeft { get; set; } = 0;
    }
}

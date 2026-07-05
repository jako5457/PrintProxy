using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintLib.Moonraker.MoonrakerDtos
{
    public class MoonrakerStatusResponse
    {
        [JsonProperty("display_status")]
        public MoonrakerDisplayStatus DisplayStatus { get; set; }

        [JsonProperty("print_stats")]
        public MoonrakerPrintStats PrintStats { get; set; }

    }

    public class MoonrakerDisplayStatus
    {
        [JsonProperty("progress")]
        public float Progess { get; set; }

        [JsonIgnore]
        public int ProgressProcent { get => Convert.ToInt32(Progess * 100); }
    }

    public class MoonrakerPrintStats
    {
        [JsonProperty("filename")]
        public string FileName { get; set; } = string.Empty;

        [JsonProperty("state")]
        public string State { get; set; } = string.Empty;

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
    }
}

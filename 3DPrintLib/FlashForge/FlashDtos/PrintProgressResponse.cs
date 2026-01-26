using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    public class PrintProgressResponse
    {
        [JsonPropertyName("progress")]
        public int Progress { get; set; }

        [JsonPropertyName("remaining_time")]
        public int RemainingTime { get; set; }

        [JsonPropertyName("elapsed_time")]
        public int ElapsedTime { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;
    }
}

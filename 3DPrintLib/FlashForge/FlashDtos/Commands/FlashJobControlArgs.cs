using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.FlashForge.FlashDtos.Commands
{
    public class FlashJobControlArgs
    {

        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

    }

    public static class FlashJobAction
    {
        public const string Pause = "pause";
        public const string Cancel = "cancel";
        public const string Continue = "continue";
    }
}

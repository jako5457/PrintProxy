using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PrintLib.FlashForge.FlashDtos.Commands
{
    public class FlashJobControlArgs
    {

        [JsonProperty("action")]
        public string Action { get; set; } = string.Empty;

    }

    public static class FlashJobAction
    {
        public const string Pause = "pause";
        public const string Cancel = "cancel";
        public const string Continue = "continue";
    }
}

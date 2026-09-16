using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3DPrintLib.BambuLab.Dtos.PrintInfo
{
    internal class BambuPrintInfo
    {
        [JsonProperty("percent")]
        public int PrintPercent { get; set; }

        [JsonProperty("gcode_state")]
        public string StatusMessage { get; set; } = string.Empty;

        [JsonProperty("subtask_name")]
        public string SubtaskName { get; set; } = string.Empty;

        [JsonProperty("ipcam")]
        public BambuIpcamInfo IpcamInfo { get; set; } = default!;
    }
}

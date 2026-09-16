using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3DPrintLib.BambuLab.Dtos.PrintInfo
{
    internal class BambuIpcamInfo
    {

        [JsonProperty("rtsp_url")]
        public string RtspUrl { get; set; } = string.Empty;

    }
}

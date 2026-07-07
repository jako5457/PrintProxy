using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3DPrintLib.Moonraker.MoonrakerDtos
{
    public class MoonrakerUploadResponse
    {
        [JsonProperty("item")]
        public MoonrakerUploadItem? Item { get; set; } = null!;

        [JsonProperty("print_started")]
        public bool PrintStarted { get; set; }

        [JsonProperty("print_queued")]
        public bool PrintQueued { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; } = string.Empty;
    }

    public class MoonrakerUploadItem
    {
        public string path { get; set; } = string.Empty;
        public string root { get; set; } = string.Empty;
        public float modified { get; set; }
        public int size { get; set; }
        public string permissions { get; set; } = string.Empty;
    }

}

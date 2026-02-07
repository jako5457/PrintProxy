using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    internal class FlashCommand<T> where T : new()
    {
        [JsonProperty("cmd")]
        public string Cmd { get; set; } = string.Empty;

        [JsonProperty("args")]
        public T Args { get; set; } = new T();
    } 
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    internal class FlashCommand<T> where T : new()
    {
        [JsonPropertyName("cmd")]
        public string Cmd { get; set; } = string.Empty;

        [JsonPropertyName("args")]
        public T Args { get; set; } = new T();
    } 
}

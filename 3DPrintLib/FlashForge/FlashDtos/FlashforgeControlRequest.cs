using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    internal class FlashforgeControlRequest<T> : FlashforgeAuthRequest where T : new()
    {

        [JsonPropertyName("payload")]
        public FlashCommand<T> PayLoad { get; set; } = new FlashCommand<T>(); 

    }
}

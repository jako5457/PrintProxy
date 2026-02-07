using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    internal class FlashforgeControlRequest<T> : FlashforgeAuthRequest where T : new()
    {

        [JsonProperty("payload")]
        public FlashCommand<T> PayLoad { get; set; } = new FlashCommand<T>(); 

    }
}

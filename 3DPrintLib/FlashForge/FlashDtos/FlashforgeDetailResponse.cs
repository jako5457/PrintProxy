using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    internal class FlashforgeDetailResponse
    {

        [JsonProperty("code")]
        public int Code { get; set; }

        public FlashforgeDetail detail { get; set; } = default!;

    }
}

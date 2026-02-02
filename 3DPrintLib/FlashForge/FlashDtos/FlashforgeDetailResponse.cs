using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    internal class FlashforgeDetailResponse
    {

        [JsonPropertyName("code")]
        public int Code { get; set; }

        public FlashforgeDetail detail { get; set; } = default!;

    }
}

using Newtonsoft.Json;
using PrintLib.FlashForge.FlashDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3DPrintLib.FlashForge.FlashDtos
{
    internal class FlashForgeMultiColorPrintRequest : FlashForgePrintRequest
    {

        [JsonProperty("useMatlStation")]
        public bool UseMatlStation { get; set; } = false;

    }
}

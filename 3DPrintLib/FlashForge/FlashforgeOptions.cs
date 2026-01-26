using System;
using System.Collections.Generic;
using System.Text;

namespace _3DPrintLib.FlashForge
{
    public class FlashforgeOptions
    {

        public required string PrinterIP { get; set; }

        public int Port { get; set; } = 8898;

        public required string AccessCode { get; set; }

        public string FullAddress { get => $"http://{PrinterIP}:{Port}"; }

    }
}

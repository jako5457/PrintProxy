using System;
using System.Collections.Generic;
using System.Text;

namespace _3DPrintLib
{
    public class PrinterEventargs : EventArgs
    {

        public string PrinterName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public int Progress { get; set; } = 0;

        public Dictionary<string, string> info = new();

    }
}

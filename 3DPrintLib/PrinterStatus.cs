using System;
using System.Collections.Generic;
using System.Text;

namespace _3DPrintLib
{
    public class PrinterStatus : JobStatus
    {
        /// <summary>
        /// The printer name
        /// </summary>
        public string PrinterName { get; set; } = string.Empty;

        /// <summary>
        /// the filename of the file printing
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Additional information about the printer
        /// </summary>
        public Dictionary<string, string> info = new();
    }
}

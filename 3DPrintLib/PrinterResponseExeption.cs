using System;
using System.Collections.Generic;
using System.Text;

namespace _3DPrintLib
{
    internal class PrinterResponseExeption : Exception
    {
        public PrinterResponseExeption(string message) : base(message) { }
    }
}

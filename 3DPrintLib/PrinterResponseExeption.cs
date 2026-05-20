using System;
using System.Collections.Generic;
using System.Text;

namespace PrintLib
{
    internal class PrinterResponseExeption : Exception
    {
        public PrinterResponseExeption(string message) : base(message) { }
    }
}

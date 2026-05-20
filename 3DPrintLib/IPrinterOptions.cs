using PrintLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintLib
{
    public interface IPrinterOptions<T> where T : class
    {
        public T GetOptions();
    }
}

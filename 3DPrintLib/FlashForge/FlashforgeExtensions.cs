using _3DPrintLib.FlashForge.FlashDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3DPrintLib.FlashForge
{
    internal static class FlashforgeExtensions
    {

        public static T SetupRequest<T>(this FlashforgeOptions options) where T : FlashforgeAuthRequest, new()
        {
            var Request = new T();

            Request.SerialNumber = options.SerialNumber;
            Request.CheckCode = options.CheckCode;

            return Request;
        }

    }
}

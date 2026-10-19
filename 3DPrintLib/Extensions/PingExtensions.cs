using PrintLib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace PrintLib.Extensions
{
    public static class PingExtensions
    {

        public static async Task<bool> PingPrinterAsync(this IPrinter printer)
        {
            var address = await printer.GetIpAddressAsync();

            if (address == null)
            {
                return true;
            }

            using Ping pinger = new Ping();

            PingOptions options = new PingOptions()
            {
                DontFragment = true
            };

            PingReply reply = await pinger.SendPingAsync(address,timeout: TimeSpan.FromSeconds(1),options: options);

            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}

using _3DPrintLib.FlashForge;
using _3DPrintLib.OctoPrint;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using PrintLib;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PrintProxy.Controller
{
    public static class PrinterSetups
    {

        public static void AddOctoPrint(this ServiceCollection services,IConfiguration config,Logger log)
        {

            string? endpoint = config.GetValue<string>("OCTOPRINT_ENDPOINT");

            if (endpoint == null)
            {
                log.Error("OCTOPRINT_ENDPOINT not present....");
                Environment.Exit(1);
            }

            string? ApiKey = config.GetValue<string>("OCTOPRINT_APIKEY");

            if (ApiKey == null)
            {
                log.Error("OCTOPRINT_APIKEY not present....");
                Environment.Exit(1);
            }

            var options = new OctoPrintOptions()
            {
                EndPoint = new Uri(endpoint),
                ApiKey = ApiKey
            };

            services.AddSingleton(_ => options);
            services.AddTransient<IPrinter, OctoPrintPrinter>();
        }

        public static void AddFlashforge(this ServiceCollection services, IConfiguration config, Logger log)
        {

            string? PrinterIp = config.GetValue<string>("FLASHFORGE_IP");

            if (PrinterIp == null)
            {
                log.Error("FLASHFORGE_IP not present....");
                Environment.Exit(1);
            }

            string? SerialNumber = config.GetValue<string>("FLASHFORGE_SERIALNUMBER");

            if (SerialNumber == null)
            {
                log.Error("FLASHFORGE_SERIALNUMBER not present....");
                Environment.Exit(1);
            }

            string? Code = config.GetValue<string>("FLASHFORGE_CODE");

            if (SerialNumber == null)
            {
                log.Error("FLASHFORGE_CODE not present....");
                Environment.Exit(1);
            }

            string? Port = config.GetValue<string>("FLASHFORGE_PORT");

            FlashforgeOptions options = new()
            {
                PrinterIP = "--",
                SerialNumber = "--",
                CheckCode = "--"
            };

            if (Port != null)
            {
                options.Port = Convert.ToInt32(Port);
            }

            services.AddSingleton(_ => options);
            services.AddTransient<IPrinter, FlashforgePrinter>();
        }

    }
}

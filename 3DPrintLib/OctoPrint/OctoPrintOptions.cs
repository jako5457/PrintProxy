using System;
using System.Collections.Generic;
using System.Text;

namespace _3DPrintLib.OctoPrint
{
    public class OctoPrintOptions : IPrinterOptions<OctoPrintOptions>
    {
        /// <summary>
        /// The endpoint of the octoprint server
        /// </summary>
        public required Uri EndPoint { get; set; }

        /// <summary>
        /// The api key of a user of octoprint
        /// </summary>
        public required string ApiKey { get; set; }

        public OctoPrintOptions GetOptions() => this;
    }
}

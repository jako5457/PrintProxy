using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintLib.OctoPrint
{
    public class OctoPrintOptions : IPrinterOptions<OctoPrintOptions>
    {
        /// <summary>
        /// The endpoint of the octoprint server
        /// </summary>
        [JsonProperty("Endpoint")]
        public required Uri EndPoint { get; set; }

        /// <summary>
        /// The api key of a user of octoprint
        /// </summary>
        [JsonProperty("Api_key")]
        public required string ApiKey { get; set; }

        public OctoPrintOptions GetOptions() => this;
    }
}

using PrintLib;
using System.Text.Json.Serialization;

namespace PrintProxy.Hub.Models
{
    public class PrinterModel
    {

        public int PrinterId { get; set; }

        public string PrinterName { get; set; } = string.Empty;

        public string PrinterIdentifier { get; set; } = string.Empty;

        [JsonIgnore]
        public IPrinter PrinterConn { get; set; } = null!;
    }
}

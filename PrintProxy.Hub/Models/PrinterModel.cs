using PrintLib;
using System.Text.Json.Serialization;

namespace PrintProxy.Hub.Models
{
    public class PrinterModel
    {

        public int PrinterId { get; set; }

        public string PrinterName { get; set; } = string.Empty;

        public string PrinterIdentifier { get; set; } = string.Empty;

        public string[] Tags = new string[0];

        [JsonIgnore]
        public IPrinter PrinterConn { get; set; } = null!;

        public override string ToString()
        {
            return PrinterName;
        }
    }
}

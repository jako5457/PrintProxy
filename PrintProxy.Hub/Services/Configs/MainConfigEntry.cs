using PrintLib.FlashForge;
using PrintLib.OctoPrint;
using System.Text.Json.Serialization;

namespace PrintProxy.Hub.Services.Configs
{
    public class MainConfigEntry
    {
        [JsonPropertyName("octoprint")]
        public List<OctoPrintOptions> Octoprint { get; set; } = new();

        [JsonPropertyName("flashforge")]
        public List<FlashforgeOptions> Flashforge { get; set; } = new();
    }
}

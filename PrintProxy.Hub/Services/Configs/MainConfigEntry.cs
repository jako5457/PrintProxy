using PrintLib.FlashForge;
using PrintLib.Moonraker;
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


        [JsonPropertyName("moonraker")]
        public List<MoonrakerOptions> Moonraker { get; set; } = new();
    }
}

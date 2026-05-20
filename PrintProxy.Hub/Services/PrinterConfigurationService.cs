using Newtonsoft.Json;
using PrintProxy.Hub.Services.Configs;

namespace PrintProxy.Hub.Services
{
    public class PrinterConfigurationService(IConfiguration _Config) : IPrinterConfigurationService
    {

        private readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        public MainConfigEntry GetConfig()
        {
            string? location = _Config.GetValue<string?>("PRINTER_CONFIG", null);

            if (location == null)
            {
                location = "PrinterConfig.json";
            }

            if (!File.Exists(location))
            {
                return new MainConfigEntry();
            }

            string json = File.ReadAllText(location);

            return JsonConvert.DeserializeObject<MainConfigEntry>(json, SerializerSettings) ?? new MainConfigEntry();
        }

        public void SaveConfig(MainConfigEntry config)
        {
            string? location = _Config.GetValue<string?>("PRINTER_CONFIG", null);

            if (location == null)
            {
                location = "PrinterConfig.json";
            }

            string json = JsonConvert.SerializeObject(config, SerializerSettings);

            File.WriteAllText(location, json);
        }
    }
}

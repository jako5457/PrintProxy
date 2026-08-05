using Newtonsoft.Json;
using PrintProxy.Hub.Services.Configs;

namespace PrintProxy.Hub.Services
{
    public class PrinterConfigurationService(IConfiguration _Config, ILogger<PrinterConfigurationService> logger) : IPrinterConfigurationService
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

            logger.LogInformation("Printer configuration is " + location);

            if (!File.Exists(location))
            {
                var conf = new MainConfigEntry();

                var data = JsonConvert.SerializeObject(conf, SerializerSettings);

                logger.LogInformation("Created config at " + location);

                File.WriteAllText(location, data);

                return conf;
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

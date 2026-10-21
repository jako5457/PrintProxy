using Newtonsoft.Json;
using PrintProxy.Hub.Services.Configs;

namespace PrintProxy.Hub.Services
{
    public class ServerOptionsService(IConfiguration _Config, ILogger<PrinterConfigurationService> logger) : IServerOptionsService
    {

        private readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        public ServerOptionsEntry GetOptions()
        {
            string? location = _Config.GetValue<string?>("SERVER_OPTIONS", null);

            if (location == null)
            {
                location = "ServerConfig.json";
            }

            //logger.LogInformation("Printer options is " + location);

            if (!File.Exists(location))
            {
                var conf = new ServerOptionsEntry();

                var data = JsonConvert.SerializeObject(conf, SerializerSettings);

                logger.LogInformation("Created options at " + location);

                File.WriteAllText(location, data);

                return conf;
            }

            string json = File.ReadAllText(location);

            return JsonConvert.DeserializeObject<ServerOptionsEntry>(json, SerializerSettings) ?? new ServerOptionsEntry();
        }

        public void SaveConfig(ServerOptionsEntry options)
        {
            string? location = _Config.GetValue<string?>("SERVER_OPTIONS", null);

            if (location == null)
            {
                location = "ServerConfig.json";
            }

            string json = JsonConvert.SerializeObject(options, SerializerSettings);

            File.WriteAllText(location, json);
        }

    }
}

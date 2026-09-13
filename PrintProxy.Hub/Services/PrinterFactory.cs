using PrintLib.FlashForge;
using PrintLib.OctoPrint;
using PrintLib;
using PrintProxy.Hub.Services.Configs;
using PrintLib.Moonraker;
using _3DPrintLib.BambuLab;
using MQTTnet;

namespace PrintProxy.Hub.Services
{
    public class PrinterFactory : IPrinterFactory
    {

        private readonly MainConfigEntry _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly MqttClientFactory _mqttClientFactory;

        public PrinterFactory(IPrinterConfigurationService configuration, IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider, MqttClientFactory mqttClientFactory)
        {
            _config = configuration.GetConfig();
            _httpClientFactory = httpClientFactory;
            _serviceProvider = serviceProvider;
            _mqttClientFactory = mqttClientFactory;
        }

        public IEnumerable<IPrinter> GetPrinters()
        {
            foreach (var octoOptions in _config.Octoprint)
            {
                yield return new OctoPrintPrinter(_httpClientFactory, octoOptions);
            }

            foreach (var flashOptions in _config.Flashforge)
            {
                yield return new FlashforgePrinter(_httpClientFactory, _serviceProvider.GetRequiredService<ILogger<FlashforgePrinter>>(), flashOptions);
            }

            foreach (var moonOptions in _config.Moonraker)
            {
                yield return new MoonrakerPrinter(moonOptions, _httpClientFactory, _serviceProvider.GetRequiredService<ILogger<MoonrakerPrinter>>());
            }

            foreach (var bambuOptions in _config.Bambu)
            {
                yield return new BambuLabPrinter(_serviceProvider.GetRequiredService<ILogger<BambuLabPrinter>>(),_mqttClientFactory,bambuOptions);
            }
        }

        public IPrinter? GetPrinterByIdentifier(string identifier)
        {
            var Octoprinteroptions = _config.Octoprint.FirstOrDefault(p => p.Identifier == identifier);

            if (Octoprinteroptions != null)
            {
                return new OctoPrintPrinter(_httpClientFactory, Octoprinteroptions);
            }

            var Flashprinteroptions = _config.Flashforge.FirstOrDefault(p => p.Identifier == identifier);

            if (Flashprinteroptions != null)
            {
                return new FlashforgePrinter(_httpClientFactory, _serviceProvider.GetRequiredService<ILogger<FlashforgePrinter>>(), Flashprinteroptions);
            }

            var MoonrakerPrinterOptions = _config.Moonraker.FirstOrDefault(p => p.Identifier == identifier);

            if (MoonrakerPrinterOptions != null)
            {
                return new MoonrakerPrinter(MoonrakerPrinterOptions, _httpClientFactory, _serviceProvider.GetRequiredService<ILogger<MoonrakerPrinter>>());
            }

            return null;
        }

    }
}

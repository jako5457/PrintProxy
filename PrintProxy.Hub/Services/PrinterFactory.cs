using PrintLib.FlashForge;
using PrintLib.OctoPrint;
using PrintLib;
using PrintProxy.Hub.Services.Configs;

namespace PrintProxy.Hub.Services
{
    public class PrinterFactory : IPrinterFactory
    {

        private readonly MainConfigEntry _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceProvider _serviceProvider;

        public PrinterFactory(IPrinterConfigurationService configuration, IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider)
        {
            _config = configuration.GetConfig();
            _httpClientFactory = httpClientFactory;
            _serviceProvider = serviceProvider;
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

            return null;
        }

    }
}

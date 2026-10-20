using PrintProxy.Hub.Services.Configs;

namespace PrintProxy.Hub.Services
{
    public interface IPrinterConfigurationService
    {
        PrinterConfigEntry GetConfig();
        void SaveConfig(PrinterConfigEntry config);
    }
}
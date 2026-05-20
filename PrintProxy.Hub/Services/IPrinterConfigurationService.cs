using PrintProxy.Hub.Services.Configs;

namespace PrintProxy.Hub.Services
{
    public interface IPrinterConfigurationService
    {
        MainConfigEntry GetConfig();
        void SaveConfig(MainConfigEntry config);
    }
}
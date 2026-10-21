using PrintProxy.Hub.Services.Configs;

namespace PrintProxy.Hub.Services
{
    public interface IServerOptionsService
    {
        ServerOptionsEntry GetOptions();
        void SaveConfig(ServerOptionsEntry options);
    }
}
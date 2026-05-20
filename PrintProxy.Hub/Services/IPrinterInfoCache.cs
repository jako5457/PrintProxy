using PrintProxy.Hub.Models;

namespace PrintProxy.Hub.Services
{
    public interface IPrinterInfoCache
    {
        Task<JobStatusModel> GetPrinterStatusAsync(string name);
        Task UpdatePrinterStatusAsync(JobStatusModel status);
    }
}
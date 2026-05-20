using PrintLib;
using PrintProxy.Hub.Models;

namespace PrintProxy.Hub.Services
{
    public class InMemoryPrinterInfoCache : IPrinterInfoCache
    {

        private List<JobStatusModel> _Statuses = new List<JobStatusModel>();

        public Task<JobStatusModel> GetPrinterStatusAsync(string name)
        {
            return Task.FromResult(_Statuses.Where(s => s.PrinterName == name).FirstOrDefault() ?? new JobStatusModel());
        }

        public Task UpdatePrinterStatusAsync(JobStatusModel status)
        {
            var oldStatus = _Statuses.Where(s => s.PrinterName == status.PrinterName).FirstOrDefault();

            if (oldStatus != null)
            {
                _Statuses.Remove(oldStatus);
            }

            _Statuses.Add(status);

            return Task.CompletedTask;
        }
    }
}

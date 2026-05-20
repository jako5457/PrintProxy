using PrintLib;

namespace PrintProxy.Hub.Models
{
    public class JobStatusModel : JobStatus
    {
        public string PrinterName { get; set; } = string.Empty;
    }
}

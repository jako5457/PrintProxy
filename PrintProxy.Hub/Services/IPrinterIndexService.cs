using PrintProxy.Hub.Models;

namespace PrintProxy.Hub.Services
{
    public interface IPrinterIndexService
    {
        Task BeginIndexingAsync();

        Task<List<PrinterModel>> GetPrintersAsync();

        Task<PrinterModel?> GetPrinterByIdAsync(int id);

        Task<PrinterModel?> GetPrinterByIdentifierAsync(string identifier);
    }
}
using PrintLib;

namespace PrintProxy.Hub.Services
{
    public interface IPrinterFactory
    {
        IEnumerable<IPrinter> GetPrinters();
    }
}
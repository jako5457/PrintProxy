namespace PrintProxy.Hub.Services.Files
{
    public interface IPrinterfileService
    {
        Task DeleteFileAsync(string filename);
        List<PrinterFileInfo> GetFiles();
        Task SaveFileAsync(Stream stream, string filename);
    }
}
namespace PrintProxy.Hub.Services.Files
{
    public interface IPrinterfileService
    {
        Task DeleteFileAsync(string filename);
        List<PrinterFileInfo> GetFiles();
        Task SaveFileAsync(FileStream stream, string filename);
    }
}
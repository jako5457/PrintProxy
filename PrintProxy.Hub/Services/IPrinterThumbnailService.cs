namespace PrintProxy.Hub.Services
{
    public interface IPrinterThumbnailService
    {
        Task<string> GetThumbnail(string url);
    }
}
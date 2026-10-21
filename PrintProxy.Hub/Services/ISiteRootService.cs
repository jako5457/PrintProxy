namespace PrintProxy.Hub.Services
{
    public interface ISiteRootService
    {
        Task<string> DownloadFileAsync(string address, string RootLocation);

        Task<string> WriteFileAsync(Stream stream, string filename, string RootLocation);
    }
}
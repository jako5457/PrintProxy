using Microsoft.Extensions.Caching.Distributed;

namespace PrintProxy.Hub.Services
{
    public interface IAppDataService
    {
        Task<string> DownloadFileAsync(string address, string directory = "");

        Task<string> WriteFileAsync(Stream stream, string filename, string directory = "");

        Task<FileInfo> GetFileAsync(string filename, string directory = "");

        Task<byte[]> GetFileDataAsync(string filename,string directory = "", bool cache = false, DistributedCacheEntryOptions? cacheOptions = null);
    }
}
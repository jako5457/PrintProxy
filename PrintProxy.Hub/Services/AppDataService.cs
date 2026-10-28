using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Http;

namespace PrintProxy.Hub.Services
{
    public class AppDataService : IAppDataService
    {

        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<AppDataService> _Logger;

        private readonly IDistributedCache _Cache;

        private string _AppDataLocation = "appdata";

        public AppDataService(IHttpClientFactory httpClientFactory,IConfiguration configuration, ILogger<AppDataService> logger, IDistributedCache cache)
        {
            _HttpClientFactory = httpClientFactory;
            _Logger = logger;
            _Cache = cache;

            string? Location = configuration.GetValue<string>("SERVER_APPDATA");

            if (Location != null)
            {
                _AppDataLocation = Location;
            }

        }

        public async Task<string> DownloadFileAsync(string address,string directory = "")
        {
            var client = _HttpClientFactory.CreateClient();

            var FileName = address.Split("/").LastOrDefault();

            var location = Path.Combine(_AppDataLocation, FileName);

            if (string.IsNullOrWhiteSpace(directory))
            {
                string FolderLocation = Path.Combine(_AppDataLocation, directory);

                if (!Directory.Exists(FolderLocation))
                {
                    Directory.CreateDirectory(FolderLocation);
                }

                location = Path.Combine(_AppDataLocation,directory,FileName);
            }

            var result = await client.GetAsync(address);

            result.EnsureSuccessStatusCode();

            using MemoryStream stream = new MemoryStream();

            await result.Content.CopyToAsync(stream);

            await File.AppendAllBytesAsync(location, stream.ToArray());

            return location;
        }

        

        public async Task<string> WriteFileAsync(Stream stream,string filename, string directory = "")
        {
            byte[] data = new byte[stream.Length];

            var location = Path.Combine(_AppDataLocation, filename);

            if (string.IsNullOrWhiteSpace(directory))
            {
                string FolderLocation = Path.Combine(_AppDataLocation, directory);

                if (!Directory.Exists(FolderLocation))
                {
                    Directory.CreateDirectory(FolderLocation);
                }

                location = Path.Combine(_AppDataLocation, directory, filename);
            }

            await stream.ReadExactlyAsync(data, 0, data.Length);

            File.WriteAllBytes(location, data);

            return location;
        }

        public async Task<FileInfo> GetFileAsync(string filename, string directory = "")
        {
            var location = Path.Combine(_AppDataLocation, filename);

            if (string.IsNullOrWhiteSpace(directory))
            {
                location = Path.Combine(_AppDataLocation, directory, filename);
            }

            return new FileInfo(location);
        }

        public async Task<byte[]> GetFileDataAsync(string filename, string directory = "",bool cache = false, DistributedCacheEntryOptions? cacheOptions = null)
        {
            var location = Path.Combine(_AppDataLocation, filename);

            if (string.IsNullOrWhiteSpace(directory))
            {
                string FolderLocation = Path.Combine(_AppDataLocation,directory);

                if (!Directory.Exists(FolderLocation))
                {
                    Directory.CreateDirectory(FolderLocation);
                }

                location = Path.Combine(_AppDataLocation, directory, filename);
            }

            if (cache)
            {
                byte[]? fromCache = await _Cache.GetAsync(filename);

                if (fromCache != null)
                {
                    return fromCache;
                }
            }

            var file = new FileInfo(location);

            using MemoryStream Memory = new MemoryStream();

            using(FileStream filestream = file.OpenRead())
            {
                await filestream.CopyToAsync(Memory);
            }

            var data = Memory.ToArray();

            if (cache)
            {
                if (cacheOptions == null)
                {
                    cacheOptions = new DistributedCacheEntryOptions()
                    {
                        SlidingExpiration = TimeSpan.FromSeconds(60)
                    };

                }

                await _Cache.SetAsync(filename, data,cacheOptions);
            }

            return data;

        }
    }
}

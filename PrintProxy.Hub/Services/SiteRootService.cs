using Microsoft.Extensions.Http;

namespace PrintProxy.Hub.Services
{
    public class SiteRootService : ISiteRootService
    {

        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<SiteRootService> _Logger;

        public SiteRootService(IHttpClientFactory httpClientFactory, ILogger<SiteRootService> logger)
        {
            _HttpClientFactory = httpClientFactory;
            _Logger = logger;
        }

        public async Task<string> DownloadFileAsync(string address, string RootLocation)
        {

            var client = _HttpClientFactory.CreateClient();

            var FileName = address.Split("/").LastOrDefault();
            
            var location = Path.Combine("wwwroot", RootLocation, FileName);

            var result = await client.GetAsync(address);

            result.EnsureSuccessStatusCode();

            using MemoryStream stream = new MemoryStream();

            await result.Content.CopyToAsync(stream);

            await File.AppendAllBytesAsync(location, stream.ToArray());

            return Path.Join(RootLocation, FileName);
        }

        public async Task<string> WriteFileAsync(Stream stream,string filename,string RootLocation)
        {
            byte[] data = new byte[stream.Length];

            var location = Path.Combine("wwwroot", RootLocation, filename);

            await stream.ReadExactlyAsync(data, 0, data.Length);

            File.WriteAllBytes(location, data);

            return Path.Combine(RootLocation, filename);
        }
    }
}

using Microsoft.Extensions.Caching.Distributed;
using System.Buffers.Text;

namespace PrintProxy.Hub.Services
{
    public class PrinterThumbnailService(IDistributedCache cache, IHttpClientFactory httpClientFactory) : IPrinterThumbnailService
    {

        DistributedCacheEntryOptions cacheoptions = new()
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };

        public async Task<string> GetThumbnail(string url)
        {
            string thumbnail = cache.GetString(url) ?? string.Empty;

            if (string.IsNullOrEmpty(thumbnail))
            {
                HttpClient client = httpClientFactory.CreateClient();

                var response = await client.GetAsync(url);

                byte[] data = await response.Content.ReadAsByteArrayAsync();
                string type = response.Content.Headers.ContentType.MediaType ?? "image/png";

                string base64 = Convert.ToBase64String(data);

                thumbnail = $"data:{type};base64,{base64}";

                cache.SetString(url, thumbnail, cacheoptions);
            }

            return thumbnail;
        }


    }
}

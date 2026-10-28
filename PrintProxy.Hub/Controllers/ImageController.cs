using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PrintProxy.Hub.Services;

namespace PrintProxy.Hub.Controllers
{
    [Route("assets/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private IAppDataService _AppDataService;
        private IConfiguration _config;

        private string ImageFolder = "Images";

        public ImageController(IAppDataService appDataService, IConfiguration config)
        {
            _AppDataService = appDataService;
            _config = config;
        }

        [HttpGet("{filename}")]
        public async Task<ActionResult> OnGetAsync(string filename)
        {

            DistributedCacheEntryOptions options = new()
            {
                SlidingExpiration = TimeSpan.FromHours(1)
            };

            var info = await _AppDataService.GetFileAsync(filename,"images");

            if (!info.Exists)
            {
                return NotFound();
            }

            var data = await _AppDataService.GetFileDataAsync(filename);

            string extension = info.Extension.Replace(".", "").ToLower();

            string mime = $"image/{extension}";

            if (extension == "svg")
            {
                mime += "+xml";
            }

            return File(data,mime);
        }
    }
}

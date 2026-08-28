using Microsoft.EntityFrameworkCore;
using PrintProxy.Hub.Data;
using PrintProxy.Hub.Data.Entities;
using PrintProxy.Hub.Services.Files;

namespace PrintProxy.Hub.Services.Tags
{
    public class TagService : ITagService
    {

        private readonly IConfiguration _Config;
        private readonly IServiceProvider _ServiceProvider;
        private readonly ILogger<TagService> _logger;

        public TagService(IConfiguration config,IServiceProvider serviceProvider, ILogger<TagService> logger)
        {
            _Config = config;
            _ServiceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task CreateTagAsync(string TagName, bool SytemTag = false)
        {
            using var scope = _ServiceProvider.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (!await context.Tags.Where(t => t.TagName == TagName).AnyAsync())
            {
                Tag tag = new Tag()
                {
                    IsSystemTag = SytemTag,
                    TagName = TagName
                };

                try
                {
                    context.Tags.Add(tag);
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError("Failed to create tag: " + e.Message, e);
                    throw;
                }
            }
        }

        public async Task<List<string>> GetPrinterTagsAsync(string Identifier)
        {
            using var scope = _ServiceProvider.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var tags = await context.Printers.Where(p => p.PrinterIdentifier == Identifier).Select(p => p.Tags).FirstOrDefaultAsync();

            if (tags == null)
            {
                return new List<string>();
            }

            return tags.Select(t => t.TagName).ToList();
        }
    }
}

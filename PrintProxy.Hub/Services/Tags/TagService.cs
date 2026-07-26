using Microsoft.EntityFrameworkCore;
using PrintProxy.Hub.Data;
using PrintProxy.Hub.Data.Entities;
using PrintProxy.Hub.Services.Files;

namespace PrintProxy.Hub.Services.Tags
{
    public class TagService : ITagService
    {

        private readonly IConfiguration _Config;
        private readonly IPrinterfileService _PrinterfileService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TagService> _logger;

        public TagService(IConfiguration config, IPrinterfileService printerfileService, ApplicationDbContext context, ILogger<TagService> logger)
        {
            _Config = config;
            _PrinterfileService = printerfileService;
            _context = context;
            _logger = logger;
        }

        public async Task CreateTagAsync(string TagName, bool SytemTag = false)
        {
            if (!await _context.Tags.Where(t => t.TagName == TagName).AnyAsync())
            {
                Tag tag = new Tag()
                {
                    IsSystemTag = SytemTag,
                    TagName = TagName
                };

                try
                {
                    _context.Tags.Add(tag);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError("Failed to create tag: " + e.Message, e);
                    throw;
                }
            }
        }
    }
}

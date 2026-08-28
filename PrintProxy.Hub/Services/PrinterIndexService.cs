using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using PrintProxy.Hub.Data;
using PrintProxy.Hub.Data.Entities;
using PrintProxy.Hub.Models;
using PrintProxy.Hub.Services.Tags;
using System.Net.NetworkInformation;

namespace PrintProxy.Hub.Services
{
    public class PrinterIndexService(IServiceProvider serviceProvider,ILogger<PrinterIndexService> logger) : IPrinterIndexService
    {

        private const string OctoPrintTagName = "OctoPrint";
        private const string FlashForgeTagName = "Flashforge";
        private const string MoonRakerTagName = "Moonraker";

        public async Task BeginIndexingAsync()
        {
            var scope = serviceProvider.CreateAsyncScope();
            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            IPrinterConfigurationService config = scope.ServiceProvider.GetRequiredService<IPrinterConfigurationService>();
            IPrinterFactory printerFactory = scope.ServiceProvider.GetRequiredService<IPrinterFactory>();
            ITagService tagService = scope.ServiceProvider.GetRequiredService<ITagService>();
            
            logger.LogInformation("Printer indexer starting");

            logger.LogInformation("Reading config.....");

            var configuration = config.GetConfig();

            logger.LogInformation("Check octoprint configs....");

            if (configuration.Octoprint.Any())
            {
                await tagService.CreateTagAsync(OctoPrintTagName, true);
            }

            if (configuration.Flashforge.Any())
            {
                await tagService.CreateTagAsync(FlashForgeTagName, true);
            }

            if (configuration.Moonraker.Any())
            {
                await tagService.CreateTagAsync(MoonRakerTagName, true);
            }

            foreach (var Octoprinter in configuration.Octoprint)
            {
                if (!await context.Printers.AnyAsync(p => p.PrinterIdentifier == Octoprinter.Identifier))
                {

                    logger.LogInformation("Octoprint Printer does not exist.. Creating...");

                    Tag? OctoTag = await context.Tags
                                                .Where(t => t.TagName == OctoPrintTagName)
                                                .FirstOrDefaultAsync();

                    if (OctoTag == null)
                    {
                        break;
                    }

                    logger.LogInformation("Getting printer info.....");

                    var printerconn = printerFactory.GetPrinterByIdentifier(Octoprinter.Identifier);

                    try
                    {
                        if (printerconn != null)
                        {
                            var status = await printerconn.GetStatusAsync();

                            Printer printer = new Printer()
                            {
                                PrinterName = status.PrinterName,
                                PrinterIdentifier = Octoprinter.Identifier,
                                Tags = new List<Tag>() { OctoTag }
                            };

                            context.Printers.Add(printer);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error occurred. Skipping printer..");
                        continue;
                    }
                }
                else
                {
                    var printer = await context.Printers.Where(p => p.PrinterIdentifier == Octoprinter.Identifier).FirstOrDefaultAsync();

                    if (printer == null)
                    {
                        continue;
                    }

                    var printerconn = printerFactory.GetPrinterByIdentifier(Octoprinter.Identifier);

                    if (printerconn == null)
                    {
                        continue;
                    }

                    var information = await printerconn.GetStatusAsync();

                    if (printer.PrinterName != information.PrinterName)
                    {
                        if (string.IsNullOrWhiteSpace(printer.PrinterName))
                        {
                            printer.PrinterName = information.PrinterName;
                        }
                    }
                }

            }

            foreach (var flashprinter in configuration.Flashforge)
            {
                if (!await context.Printers.AnyAsync(p => p.PrinterIdentifier == flashprinter.Identifier))
                {
                    Tag? FlashForgeTag = await context.Tags
                                                      .Where(t => t.TagName == FlashForgeTagName)
                                                      .FirstOrDefaultAsync();

                    if (FlashForgeTag == null)
                    {
                        break;
                    }
    
                    var printerconn = printerFactory.GetPrinterByIdentifier(flashprinter.Identifier);

                    if (printerconn != null)
                    {
                        var status = await printerconn.GetStatusAsync();

                        Printer printer = new Printer()
                        {
                            PrinterName = status.PrinterName,
                            PrinterIdentifier = flashprinter.Identifier,
                            Tags = new List<Tag>() { FlashForgeTag }
                        };

                        context.Printers.Add(printer);
                    }
                }
                else
                {
                    var printer = await context.Printers.Where(p => p.PrinterIdentifier == flashprinter.Identifier).FirstOrDefaultAsync();

                    if (printer == null)
                    {
                        continue;
                    }

                    var printerconn = printerFactory.GetPrinterByIdentifier(flashprinter.Identifier);

                    if (printerconn == null)
                    {
                        continue;
                    }

                    var information = await printerconn.GetStatusAsync();

                    if (printer.PrinterName != information.PrinterName)
                    {
                        if (string.IsNullOrWhiteSpace(printer.PrinterName))
                        {
                           printer.PrinterName = information.PrinterName; 
                        }
                    }
                }
            }

            foreach (var moonraker in configuration.Moonraker)
            {
                if (!await context.Printers.AnyAsync(p => p.PrinterIdentifier == moonraker.Identifier))
                {

                    Tag? MoonRakerTag = await context.Tags
                                                     .Where(t => t.TagName == MoonRakerTagName)
                                                     .FirstOrDefaultAsync();

                    if (MoonRakerTag == null)
                    {
                        break;
                    }

                    var printerconn = printerFactory.GetPrinterByIdentifier(moonraker.Identifier);

                    if (printerconn != null)
                    {
                        var status = await printerconn.GetStatusAsync();

                        Printer printer = new Printer()
                        {
                            PrinterName = status.PrinterName,
                            PrinterIdentifier = moonraker.Identifier,
                            Tags = new List<Tag>() { MoonRakerTag }
                        };

                        context.Printers.Add(printer);
                    }
                }
                else
                {
                    var printer = await context.Printers.Where(p => p.PrinterIdentifier == moonraker.Identifier).FirstOrDefaultAsync();

                    if (printer == null)
                    {
                        continue;
                    }

                    var printerconn = printerFactory.GetPrinterByIdentifier(moonraker.Identifier);

                    if (printerconn == null)
                    {
                        continue;
                    }

                    var information = await printerconn.GetStatusAsync();

                    if (printer.PrinterName != information.PrinterName)
                    {
                        if (string.IsNullOrWhiteSpace(printer.PrinterName))
                        {
                            printer.PrinterName = information.PrinterName;
                        }
                    }
                }
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Indexing failed...");
                Environment.Exit(0);
            }
        }

        public async Task<List<PrinterModel>> GetPrintersAsync()
        {
            
            var scope = serviceProvider.CreateAsyncScope();
            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            IPrinterFactory printerFactory = scope.ServiceProvider.GetRequiredService<IPrinterFactory>();
            
            var printers = await context.Printers.Select(p => new PrinterModel()
            {
               PrinterId = p.PrinterId,
               PrinterName = p.PrinterName,
               PrinterConn = printerFactory.GetPrinterByIdentifier(p.PrinterIdentifier) ?? null!,
               Tags = p.Tags.Select(t => t.TagName).ToArray()
            }).ToListAsync();

            return printers.Where(p => p.PrinterConn != null).ToList();
        }

        public async Task<PrinterModel?> GetPrinterByIdAsync(int id)
        {
            var scope = serviceProvider.CreateAsyncScope();
            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            IPrinterFactory printerFactory = scope.ServiceProvider.GetRequiredService<IPrinterFactory>();

            return await context.Printers.Select(p => new PrinterModel()
            {
                PrinterId = p.PrinterId,
                PrinterName = p.PrinterName,
                PrinterConn = printerFactory.GetPrinterByIdentifier(p.PrinterIdentifier) ?? null!,
                Tags = p.Tags.Select(t => t.TagName).ToArray()
            })
            .Where(p => p.PrinterId == id)
            .FirstOrDefaultAsync();
        }

        public async Task<PrinterModel?> GetPrinterByIdentifierAsync(string identifier)
        {
            var scope = serviceProvider.CreateAsyncScope();
            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            IPrinterFactory printerFactory = scope.ServiceProvider.GetRequiredService<IPrinterFactory>();
            
            return await context.Printers.Select(p => new PrinterModel()
            {
                PrinterId = p.PrinterId,
                PrinterName = p.PrinterName,
                PrinterIdentifier = p.PrinterIdentifier,
                PrinterConn = printerFactory.GetPrinterByIdentifier(p.PrinterIdentifier) ?? null!,
                Tags = p.Tags.Select(t => t.TagName).ToArray()
            })
            .Where(p => p.PrinterIdentifier == identifier)
            .FirstOrDefaultAsync();
        }
    }
}

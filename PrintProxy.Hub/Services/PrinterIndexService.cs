using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using PrintProxy.Hub.Data;
using PrintProxy.Hub.Data.Entities;
using PrintProxy.Hub.Models;

namespace PrintProxy.Hub.Services
{
    public class PrinterIndexService(ApplicationDbContext context, IPrinterConfigurationService config, ILogger<PrinterIndexService> logger, IPrinterFactory printerFactory) : IPrinterIndexService
    {
        public async Task BeginIndexingAsync()
        {
            logger.LogInformation("Printer indexer starting");

            logger.LogInformation("Reading config.....");

            var configuration = config.GetConfig();

            logger.LogInformation("Check octoprint configs....");

            foreach (var Octoprinter in configuration.Octoprint)
            {
                if (!await context.Printers.AnyAsync(p => p.PrinterIdentifier == Octoprinter.Identifier))
                {

                    logger.LogInformation("Octoprint Printer does not exist.. Creating...");

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
                                PrinterIdentifier = Octoprinter.Identifier
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

                foreach (var flashprinter in configuration.Flashforge)
                {
                    if (!await context.Printers.AnyAsync(p => p.PrinterIdentifier == flashprinter.Identifier))
                    {
                        var printerconn = printerFactory.GetPrinterByIdentifier(flashprinter.Identifier);

                        if (printerconn != null)
                        {
                            var status = await printerconn.GetStatusAsync();

                            Printer printer = new Printer()
                            {
                                PrinterName = status.PrinterName,
                                PrinterIdentifier = flashprinter.Identifier
                            };

                            context.Printers.Add(printer);
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
        }

        public async Task<List<PrinterModel>> GetPrintersAsync()
        {
            var printers = await context.Printers.Select(p => new PrinterModel()
            {
               PrinterId = p.PrinterId,
               PrinterName = p.PrinterName,
               PrinterConn = printerFactory.GetPrinterByIdentifier(p.PrinterIdentifier) ?? null!
            }).ToListAsync();

            return printers.Where(p => p.PrinterConn != null).ToList();
        }

        public async Task<PrinterModel?> GetPrinterByIdAsync(int id)
        {
            return await context.Printers.Select(p => new PrinterModel()
            {
                PrinterId = p.PrinterId,
                PrinterName = p.PrinterName,
                PrinterConn = printerFactory.GetPrinterByIdentifier(p.PrinterIdentifier) ?? null!
            })
            .Where(p => p.PrinterId == id)
            .FirstOrDefaultAsync();
        }

        public async Task<PrinterModel?> GetPrinterByIdentifierAsync(string identifier)
        {
            return await context.Printers.Select(p => new PrinterModel()
            {
                PrinterId = p.PrinterId,
                PrinterName = p.PrinterName,
                PrinterIdentifier = p.PrinterIdentifier,
                PrinterConn = printerFactory.GetPrinterByIdentifier(p.PrinterIdentifier) ?? null!
            })
            .Where(p => p.PrinterIdentifier == identifier)
            .FirstOrDefaultAsync();
        }
    }
}

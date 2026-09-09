using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MudBlazor;
using Newtonsoft.Json;
using PrintLib;
using PrintProxy.Hub.Data.Entities;
using PrintProxy.Hub.Models;
using PrintProxy.Hub.Services;
using System.Net.NetworkInformation;

namespace PrintProxy.Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrintersController : ControllerBase
    {

        private readonly IPrinterFactory _PrinterFactory;
        private readonly IPrinterIndexService _IndexService;

        private readonly IPrinterThumbnailService _PrinterThumbnailService;

        public PrintersController(IPrinterFactory printerFactory, IPrinterIndexService indexService,IPrinterThumbnailService printerThumbnailService)
        {
            _PrinterFactory = printerFactory;
            _IndexService = indexService;
            _PrinterThumbnailService = printerThumbnailService;
        }

        public async Task<ActionResult<List<PrinterModel>>> GetAsync()
        {
            var printerconns = _PrinterFactory.GetPrinters();

            List<PrinterModel> Printers = new List<PrinterModel>();
            
            foreach (var printer in printerconns)
            {
                PrinterModel? model = await _IndexService.GetPrinterByIdentifierAsync(printer.GetIdentifier());
                try
                {
                    var status = await printer.GetStatusAsync();

                    if (model == null)
                    {
                        model = new PrinterModel();
                    }

                    model.PrinterStatus = status.Status;
                    model.PrinterName = model?.PrinterName ?? "Printer";

                    if (!string.IsNullOrWhiteSpace(status.FileThumbnail))
                    {
                        model.PrinterFileThumbnail = await _PrinterThumbnailService.GetThumbnail(status.FileThumbnail);
                    }

                    if (model != null)
                    {
                        Printers.Add(model as PrinterModel);
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            return Printers;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using PrintProxy.Hub.Data.Entities;
using PrintProxy.Hub.Models;
using PrintProxy.Hub.Services;

namespace PrintProxy.Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrinterController : ControllerBase
    {

        private readonly IPrinterFactory _PrinterFactory;
        private readonly IPrinterIndexService _IndexService;
        private readonly ILogger<PrinterController> _Logger;
        private readonly IPrinterThumbnailService _PrinterThumbnailService;

        public PrinterController(IPrinterFactory printerFactory, IPrinterIndexService indexService,IPrinterThumbnailService printerThumbnailService, ILogger<PrinterController> logger)
        {
            _PrinterFactory = printerFactory;
            _IndexService = indexService;
            _Logger = logger;
            _PrinterThumbnailService = printerThumbnailService;
        }

        public async Task<ActionResult<PrinterModel>> OnGet([FromQuery] PrinterSearchOptionsModel options)
        {
            PrinterModel? model = null!;

            if (options.id != 0)
            {
                model = await _IndexService.GetPrinterByIdAsync(options.id);
            }
            else if(!string.IsNullOrWhiteSpace(options.identifier))
            {
                model = await _IndexService.GetPrinterByIdentifierAsync(options.identifier);
            }
            else
            {
                return NotFound("Printer not specified");
            }

            if (model == null)
            {
                return NotFound();
            }

            try
            {
                var status = await model.PrinterConn.GetStatusAsync();

                if (model == null)
                {
                    model = new PrinterModel();
                }

                model.PrinterStatus = status.Status;
                model.PrinterName = model?.PrinterName ?? "Printer";

                if (!string.IsNullOrWhiteSpace(status.FileThumbnail))
                {
                    model?.PrinterFileThumbnail = await _PrinterThumbnailService.GetThumbnail(status.FileThumbnail);
                }
            }
            catch (Exception e)
            {
                _Logger.LogError("Failed to get info from printer: {0}",e.Message);
            }

            return model;
        }

        public class PrinterSearchOptionsModel
        {
            public string identifier { get; set; } = string.Empty;

            public int id { get; set; } = 0;
        }
    }
}

// See https://aka.ms/new-console-template for more information
using _3DPrintLib;
using _3DPrintLib.OctoPrint;
using Dumpify;
using Microsoft.Extensions.DependencyInjection;
using PrintLib;


var options = new OctoPrintOptions()
{
    EndPoint = new Uri("http://localhost:8080"),
    ApiKey = "NrtXK0RlwAw0-WqJ1_tUHzIZxTiVo-Qvz7xBzwmAJjg"
};

var services = new ServiceCollection();

services.AddHttpClient();
services.AddTransient<IPrinter, OctoPrintPrinter>(sp => new OctoPrintPrinter(sp.GetRequiredService<IHttpClientFactory>(), options));

var provider = services.BuildServiceProvider();

Console.WriteLine("Printer Test");

await Task.Delay(10000);

var printer = provider.GetRequiredService<IPrinter>();

Console.WriteLine("______ GET INFO ______");
var info = await printer.GetStatusAsync();

info.Dump();

Console.WriteLine("Additional info:");
foreach (var item in info.info)
{
    Console.WriteLine($"- {item.Key}: {item.Value}");
}

await Task.Delay(10000);

Console.WriteLine("______ Uploading TestFile.gcode  ______");

await printer.UploadAsync("TestFile.gcode");

Console.WriteLine("______ Upload Completed. ______");

Console.WriteLine("______ GET JOB Before Print______");
await GetPrinterJobDumpAsync(printer);

await Task.Delay(10000);

Console.WriteLine("______ Start Print ______");

await printer.StartAsync("TestFile.gcode");

await GetPrinterJobDumpAsync(printer);

await Task.Delay(20000);

Console.WriteLine("______ Pause Print ______");

await printer.PauseAsync();

await GetPrinterJobDumpAsync(printer);

await Task.Delay(20000);

Console.WriteLine("______ Continue Print ______");

await printer.ContinueAsync();

await GetPrinterJobDumpAsync(printer);

await Task.Delay(20000);

Console.WriteLine("______ Stop Print ______");

await printer.StopAsync();

await GetPrinterJobDumpAsync(printer);


async Task GetPrinterJobDumpAsync(IPrinter printer)
{
    var job = await printer.GetJobStatusAsync();

    job.Dump();
}
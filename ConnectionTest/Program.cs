// See https://aka.ms/new-console-template for more information
using _3DPrintLib;
using _3DPrintLib.FlashForge;
using _3DPrintLib.OctoPrint;
using Dumpify;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PrintLib;
using System.Reflection;


var services = new ServiceCollection();

services.AddHttpClient();

// Global test options
string GcodeFile = "3DBenchy.gcode";

#region OctoPrint

var options = new OctoPrintOptions()
{
    EndPoint = new Uri("http://localhost:8080"),
    ApiKey = "--"
};

services.AddSingleton(sp => options);
//services.AddTransient<IPrinter, OctoPrintPrinter>();

#endregion OctoPrint

#region Flashforge API

FlashforgeOptions FlashOptions = new()
{
    PrinterIP = "--",
    SerialNumber = "--",
    CheckCode = "--"
};

services.AddSingleton(sp => FlashOptions);
services.AddTransient<IPrinter, FlashforgePrinter>();
services.AddLogging(options => options.AddConsole().SetMinimumLevel(LogLevel.Trace));
#endregion

var provider = services.BuildServiceProvider();

// Run
Console.WriteLine("_____ Printer Test _____");

var printer = provider.GetRequiredService<IPrinter>();

await GetInfo(printer);

await Task.Delay(10000);

Console.WriteLine("______ GET JOB Before Print______");
await GetPrinterJobDumpAsync(printer);

//await Task.Delay(10000);
//await UploadFile(printer);
//await Task.Delay(20000);
await StartPrint(printer);
await Task.Delay(20000);
await PausePrint(printer);
await Task.Delay(20000);
await ResumePrint(printer);
await Task.Delay(20000);
await StopPrint(printer);
Console.ReadLine();


async Task GetInfo(IPrinter printer)
{
    Console.WriteLine("______ GET INFO ______");
    var info = await printer.GetStatusAsync();

    info.Dump();

    Console.WriteLine("Additional info:");
    foreach (var item in info.info)
    {
        Console.WriteLine($"- {item.Key}: {item.Value}");
    }
}

async Task UploadFile(IPrinter Printer)
{
    Console.WriteLine("______ Uploading TestFile.gcode  ______");

    var printer = provider.GetRequiredService<IPrinter>();

    await printer.UploadAsync(GcodeFile);

    Console.WriteLine("______ Upload Completed. ______");
}

async Task StartPrint(IPrinter printer)
{
    Console.WriteLine("______ Start Print ______");

    await printer.StartAsync(GcodeFile);

    await GetPrinterJobDumpAsync(printer);
}

async Task PausePrint(IPrinter printer)
{
    Console.WriteLine("______ Pause Print ______");

    await printer.PauseAsync();

    await GetPrinterJobDumpAsync(printer);
}

async Task ResumePrint(IPrinter printer)
{
    Console.WriteLine("______ Continue Print ______");

    await printer.ContinueAsync();

    await GetPrinterJobDumpAsync(printer);
}

async Task StopPrint(IPrinter printer)
{
    Console.WriteLine("______ Stop Print ______");

    await printer.StopAsync();

    await GetPrinterJobDumpAsync(printer);
}

async Task GetPrinterJobDumpAsync(IPrinter printer)
{
    var job = await printer.GetJobStatusAsync();

    job.Dump();
}
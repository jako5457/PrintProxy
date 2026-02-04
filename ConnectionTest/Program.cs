// See https://aka.ms/new-console-template for more information
using _3DPrintLib;
using _3DPrintLib.FlashForge;
using _3DPrintLib.OctoPrint;
using Dumpify;
using Microsoft.Extensions.DependencyInjection;
using PrintLib;
using System.Reflection;


var services = new ServiceCollection();

services.AddHttpClient();

// Global test options
string GcodeFile = "TestFile.gcode";

#region OctoPrint

var options = new OctoPrintOptions()
{
    EndPoint = new Uri("http://localhost:8080"),
    ApiKey = "NrtXK0RlwAw0-WqJ1_tUHzIZxTiVo-Qvz7xBzwmAJjg"
};

services.AddSingleton(sp => options);
//services.AddTransient<IPrinter, OctoPrintPrinter>();

#endregion OctoPrint

#region Flashforge API

FlashforgeOptions FlashOptions = new()
{
    PrinterIP = "127.0.0.1",
    SerialNumber = "123456ABC",
    CheckCode = "1234567"
};

services.AddSingleton(sp => FlashOptions);
services.AddTransient<IPrinter, FlashforgePrinter>();

#endregion

var provider = services.BuildServiceProvider();

// Run
Console.WriteLine("Printer Test");

var printer = provider.GetRequiredService<IPrinter>();

await GetInfo(printer);

Console.WriteLine("______ GET JOB Before Print______");
await GetPrinterJobDumpAsync(printer);
await Task.Delay(10000);

//await UploadFile(printer);

await StartPrint(printer);
await PausePrint(printer);
await ResumePrint(printer);
await StopPrint(printer);


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

    await Task.Delay(10000);
}

async Task UploadFile(IPrinter Printer)
{
    Console.WriteLine("______ Uploading TestFile.gcode  ______");

    var printer = provider.GetRequiredService<IPrinter>();

    await printer.UploadAsync(GcodeFile);

    Console.WriteLine("______ Upload Completed. ______");

    await Task.Delay(10000);
}

async Task StartPrint(IPrinter printer)
{
    Console.WriteLine("______ Start Print ______");

    await printer.StartAsync(GcodeFile);

    await GetPrinterJobDumpAsync(printer);

    await Task.Delay(20000);
}

async Task PausePrint(IPrinter printer)
{
    Console.WriteLine("______ Pause Print ______");

    await printer.PauseAsync();

    await GetPrinterJobDumpAsync(printer);

    await Task.Delay(20000);
}

async Task ResumePrint(IPrinter printer)
{
    Console.WriteLine("______ Continue Print ______");

    await printer.ContinueAsync();

    await GetPrinterJobDumpAsync(printer);

    await Task.Delay(20000);
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
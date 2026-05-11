using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrintProxy.Controller;
using Serilog;

using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var config = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .Build();

log.Information("PrintProxy Controller is starting up.");

var services = new ServiceCollection();

services.AddSingleton<IConfiguration>(_ => config);
services.AddHttpClient();
services.AddSerilog(log);

string? printerType = config.GetValue<string>("PRINTER_TYPE");

if(printerType == null)
{
    log.Error("PRINTER_TYPE not present....");
    Environment.Exit(1);
}

switch (printerType.ToLower())
{
    case "flashforge":
        services.AddFlashforge(config, log);
        break;
    case "octoprint":
        services.AddOctoPrint(config, log);
        break;
    default:
        log.Error("Printer type Invalid.");
        Environment.Exit(1);
        break;
}


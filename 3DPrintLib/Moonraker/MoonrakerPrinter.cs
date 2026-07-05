using Microsoft.Extensions.Logging;
using PrintLib;
using PrintLib.FlashForge;
using System;
using System.Collections.Generic;
using System.Text;
using PrintLib.Moonraker.MoonrakerDtos;
using Newtonsoft.Json;

namespace PrintLib.Moonraker
{
    internal class MoonrakerPrinter : IPrinter
    {

        public MoonrakerOptions _Options;
        private readonly IHttpClientFactory _ClientFactory;
        private readonly ILogger<FlashforgePrinter> _Logger;

        public MoonrakerPrinter(MoonrakerOptions options, IHttpClientFactory clientFactory, ILogger<FlashforgePrinter> logger)
        {
            _Options = options;
            _ClientFactory = clientFactory;
            _Logger = logger;
        }

        public async Task ContinueAsync()
        {
            HttpClient client = _ClientFactory.CreateClient();
            client.BaseAddress = new Uri(_Options.Endpoint);

            var response = await client.PostAsync($"/printer/print/resume", null);

            string message = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _Logger.LogInformation($"Command Success responded with: {message}");
            }
            else
            {
                _Logger.LogError($"Failed to send command: {message}");
            }
        }

        public string GetIdentifier()
        {
            throw new NotImplementedException();
        }

        public async Task<JobStatus> GetJobStatusAsync() => await GetStatusAsync();

        public async Task<PrinterStatus> GetStatusAsync()
        {
            HttpClient client = _ClientFactory.CreateClient();
            client.BaseAddress = new Uri(_Options.Endpoint);
            
            var response = await client.GetAsync("/printer/objects/query?display_status&print_stats");

            string data = await response.Content.ReadAsStringAsync();

            MoonrakerStatusResponse? status = JsonConvert.DeserializeObject<MoonrakerStatusResponse>(data);

            if (status == null)
            {
                return new PrinterStatus() { Status = "Offline" };
            }

            return new PrinterStatus
            {
                FileName = status.PrintStats.FileName,
                Identifier = GetIdentifier(),
                Status = status.PrintStats.State,
                PrinterName = _Options.PrinterName,
                Progress = status.DisplayStatus.ProgressProcent,
                FileThumbnail = ""
            };

        }

        public async Task PauseAsync()
        {
            HttpClient client = _ClientFactory.CreateClient();
            client.BaseAddress = new Uri(_Options.Endpoint);

            var response = await client.PostAsync($"/printer/print/pause", null);

            string message = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _Logger.LogInformation($"Command Success responded with: {message}");
            }
            else
            {
                _Logger.LogError($"Failed to send command: {message}");
            }
        }

        public async Task StartAsync(string fileName)
        {
            HttpClient client = _ClientFactory.CreateClient();
            client.BaseAddress = new Uri(_Options.Endpoint);

            var response = await client.PostAsync($"/printer/print/start?filename={fileName}", null);

            string message = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _Logger.LogInformation($"Command Success responded with: {message}");
            }
            else
            {
                _Logger.LogError($"Failed to send command: {message}");
            }
        }

        public async Task StopAsync()
        {
            HttpClient client = _ClientFactory.CreateClient();
            client.BaseAddress = new Uri(_Options.Endpoint);

            var response = await client.PostAsync($"/printer/print/cancel", null);

            string message = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _Logger.LogInformation($"Command Success responded with: {message}");
            }
            else
            {
                _Logger.LogError($"Failed to send command: {message}");
            }
        }

        public Task UploadAsync(string FilePath)
        {
            throw new NotImplementedException();
        }
    }
}

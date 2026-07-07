using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PrintLib;
using PrintLib.FlashForge;
using PrintLib.FlashForge.FlashDtos;
using PrintLib.Moonraker.MoonrakerDtos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PrintLib.Moonraker
{
    public class MoonrakerPrinter : IPrinter
    {

        public MoonrakerOptions _Options;
        private readonly IHttpClientFactory _ClientFactory;
        private readonly ILogger<MoonrakerPrinter> _Logger;

        public MoonrakerPrinter(MoonrakerOptions options, IHttpClientFactory clientFactory, ILogger<MoonrakerPrinter> logger)
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
            using SHA256 sha = SHA256.Create();

            byte[] data = Encoding.UTF8.GetBytes($"{_Options.Endpoint}:{_Options.PrinterName}");

            byte[] identifier = sha.ComputeHash(data);

            return Convert.ToBase64String(identifier);
        }

        public async Task<JobStatus> GetJobStatusAsync() => await GetStatusAsync();

        public async Task<PrinterStatus> GetStatusAsync()
        {
            HttpClient client = _ClientFactory.CreateClient();
            client.BaseAddress = new Uri(_Options.Endpoint);
            
            var response = await client.GetAsync("/printer/objects/query?display_status&print_stats");

            string data = await response.Content.ReadAsStringAsync();

            MoonRakerStatusResponse? status = JsonConvert.DeserializeObject<MoonRakerStatusResponse>(data);

            if (status == null)
            {
                return new PrinterStatus() { Status = "Offline" };
            }

            return new PrinterStatus
            {
                FileName = status.result.status.print_stats.filename,
                Identifier = GetIdentifier(),
                Status = status.result.status.print_stats.state,
                PrinterName = _Options.PrinterName,
                Progress = Convert.ToInt32(status.result.status.display_status.progress * 100),
                FileThumbnail = $"{_Options.Endpoint}/server/files/gcodes/.thumbs/{Uri.EscapeDataString(status.result.status.print_stats.filename.Replace(".gcode",""))}-300x300.png"
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

        public async Task UploadAsync(string FilePath)
        {
            try
            {
                HttpClient client = _ClientFactory.CreateClient();
                client.BaseAddress = new Uri(_Options.Endpoint);

                FileInfo file = new FileInfo(FilePath);

                _Logger.LogInformation("Sending File: " + file.Name);

                var formData = new MultipartFormDataContent();

                formData.Headers.Add("fileSize", file.Length.ToString());

                var fileContent = new ByteArrayContent(File.ReadAllBytes(FilePath));

                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = file.Name
                };

                formData.Add(fileContent);

                var result = await client.PostAsync("/server/files/upload", formData);

                result.EnsureSuccessStatusCode();

                var json = await result.Content.ReadAsStringAsync();

                var Response = JsonConvert.DeserializeObject<FlashforgeStatusResponse>(json);

                if (Response?.Code < 0)
                {
                    throw new PrinterResponseExeption(Response.message);
                }

                _Logger.LogInformation("Printer Upload Response" + json);

            }
            catch (Exception e)
            {
                _Logger.LogError(e, $"Upload to printer failed: {e.Message}");
            }

        }
    }
}

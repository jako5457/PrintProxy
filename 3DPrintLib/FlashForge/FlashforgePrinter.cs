using PrintLib.FlashForge.FlashDtos;
using PrintLib.FlashForge.FlashDtos.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PrintLib;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrintLib.FlashForge
{
    public class FlashforgePrinter : IPrinter
    {
        private readonly IHttpClientFactory _ClientFactory;
        private readonly ILogger<FlashforgePrinter> _Logger;
        private readonly FlashforgeOptions _Options;

        public FlashforgePrinter(IHttpClientFactory clientFactory, ILogger<FlashforgePrinter> logger, FlashforgeOptions options)
        {
            _ClientFactory = clientFactory;
            _Logger = logger;
            _Options = options;
        }

        public async Task ContinueAsync()
        {
            try
            {
                var client = SetupClient();

                var request = _Options.SetupRequest<FlashforgeControlRequest<FlashJobControlArgs>>();

                request.PayLoad.Cmd = "jobCtl_cmd";
                request.PayLoad.Args.Action = FlashJobAction.Continue;

                string json = JsonConvert.SerializeObject(request);

                var msg = new HttpRequestMessage(HttpMethod.Post, "/control");

                msg.Content = new StringContent(json);
                msg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = await client.SendAsync(msg);

                result.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Error while trying continue a print" + e.Message);
            }
        }  

        public async Task<JobStatus> GetJobStatusAsync()
        {
            try
            {
                var client = SetupClient();

                var request = _Options.SetupRequest<FlashforgeAuthRequest>();

                var result = await client.PostAsJsonAsync("/detail", request);

                result.EnsureSuccessStatusCode();

                var json = await result.Content.ReadAsStringAsync();

                var Response = JsonConvert.DeserializeObject<FlashforgeDetailResponse>(json);

                return new JobStatus()
                {
                    Status = Response?.detail?.Status ?? "N/A",
                    Progress = Convert.ToInt32(Response?.detail?.PrintProgress ?? 0)
                };
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Error while trying to get printer details: " + e.Message);
                return new PrinterStatus();
            }
        }

        public async Task<PrinterStatus> GetStatusAsync()
        {
            try
            {
                var client = SetupClient();

                var request = _Options.SetupRequest<FlashforgeAuthRequest>();

                var result = await client.PostAsJsonAsync("/detail", request);

                result.EnsureSuccessStatusCode();

                var json = await result.Content.ReadAsStringAsync();

                _Logger.LogDebug(json);

                var Response = JsonConvert.DeserializeObject<FlashforgeDetailResponse>(json);

                if (Response?.Code < 0)
                {
                    throw new PrinterResponseExeption(Response.message);
                }

                return new PrinterStatus()
                {
                    PrinterName = Response?.detail?.Name ?? "Flashforge Printer",
                    FileName = Response?.detail?.PrintFileName ?? "N/A",
                    FileThumbnail = Response?.detail?.PrintFileThumbUrl ?? null,
                    PrinterCam = Response?.detail?.CameraStreamUrl ?? null,
                    Status = Response?.detail?.Status ?? "N/A",
                    Progress = Convert.ToInt32(Response?.detail?.PrintProgress ?? 0),
                    Identifier = _Options.Identifier,
                    info = new()
                    {
                        {"Estimated time",Response?.detail?.EstimatedTime ?? "N/A" },
                        {"Current print speed",Response?.detail?.CurrentPrintSpeed ?? "N/A"}
                    }
                };

                
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Error while trying to get printer details: " + e.Message);
                throw new PrinterResponseExeption("Error while trying to get printer details:" + e.Message);
            }
        }

        public async Task PauseAsync()
        {
            try
            {
                var client = SetupClient();

                var request = _Options.SetupRequest<FlashforgeControlRequest<FlashJobControlArgs>>();

                request.PayLoad.Cmd = "jobCtl_cmd";
                request.PayLoad.Args.Action = FlashJobAction.Pause;

                string json = JsonConvert.SerializeObject(request);

                var msg = new HttpRequestMessage(HttpMethod.Post, "/control");

                msg.Content = new StringContent(json);
                msg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = await client.SendAsync(msg);

                result.EnsureSuccessStatusCode();

                _Logger.LogDebug(json);
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Error while trying pause a print" + e.Message);
            }
        }

        public async Task StartAsync(string fileName)
        {
            try
            {
                var client = SetupClient();

                var request = _Options.SetupRequest<FlashForgePrintRequest>();

                request.FileName = fileName;

                var result = await client.PostAsJsonAsync("/printGcode", request);

                result.EnsureSuccessStatusCode();

                string json = await result.Content.ReadAsStringAsync();
                _Logger.LogDebug(json);
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Error while trying start a print" + e.Message);
            }
        }

        public async Task StopAsync()
        {
            try
            {
                var client = SetupClient();

                var request = _Options.SetupRequest<FlashforgeControlRequest<FlashJobControlArgs>>();

                request.PayLoad.Cmd = "jobCtl_cmd";
                request.PayLoad.Args.Action = FlashJobAction.Cancel;

                string json = JsonConvert.SerializeObject(request);

                var msg = new HttpRequestMessage(HttpMethod.Post, "/control");

                msg.Content = new StringContent(json);
                msg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = await client.SendAsync(msg);

                result.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Error while trying stop a print" + e.Message);
            }
        }

        public async Task UploadAsync(string FilePath)
        {
            try
            {
                var client = SetupClient();

                FileInfo file = new FileInfo(FilePath);

                _Logger.LogInformation("Sending File: " + file.Name);

                var formData = new MultipartFormDataContent();
                //formData.Add(new StringContent(_Options.SerialNumber), "serialNumber");
                //formData.Add(new StringContent(_Options.CheckCode), "checkCode");
                //formData.Add(new StringContent("false"), "levelingBeforePrint");

                formData.Headers.Add("serialNumber", _Options.SerialNumber);
                formData.Headers.Add("checkCode", _Options.CheckCode);
                formData.Headers.Add("levelingBeforePrint", "false");
                formData.Headers.Add("fileSize", file.Length.ToString());

                var fileContent = new ByteArrayContent(File.ReadAllBytes(FilePath));

                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "gcodeFile",
                    FileName = file.Name
                };

                formData.Add(fileContent);

                var result = await client.PostAsync("/uploadGcode", formData);

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

        private HttpClient SetupClient()
        {
            var client = _ClientFactory.CreateClient();
            client.BaseAddress = new Uri(_Options.FullAddress);

            return client;
        }

    }
}

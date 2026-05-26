using PrintLib.OctoPrint.OctoDtos;
using PrintLib;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace PrintLib.OctoPrint
{
    public class OctoPrintPrinter : IPrinter
    {

        private OctoPrintOptions _options;
        private readonly IHttpClientFactory _clientFactory;

        public OctoPrintPrinter(IHttpClientFactory clientFactory,OctoPrintOptions options)
        {
            _clientFactory = clientFactory;
            _options = options;
        }

        public async Task ContinueAsync()
        {
            HttpClient client = _clientFactory.CreateAuthorizedClient(_options);

            var response = await client.SendOctoprintJobCommandAsync(new OctoprintActionCommand("pause","resume"));

            response.EnsureSuccessStatusCode();
        }

        public async Task<JobStatus> GetJobStatusAsync()
        {
            HttpClient client = _clientFactory.CreateAuthorizedClient(_options);

            var response = await client.GetAsync("/api/job");

            response.EnsureSuccessStatusCode();

            OctoPrintJobInfo? info = await response.Content.ReadFromJsonAsync<OctoPrintJobInfo>();

            if (info == null)
            {
                throw new Exception("Printer returned invalid response.");
            }

            JobStatus status = new()
            {
                Status = info.State,
                Progress = CalculateProgress(Convert.ToInt64(info.Progress.PrintTime),Convert.ToInt64(info.Progress.PrintTimeLeft))
            };

            return status;
        }

        public async Task<PrinterStatus> GetStatusAsync()
        {
            HttpClient client = _clientFactory.CreateAuthorizedClient(_options);

            PrinterStatus status = new();

            #region Get Server Info
            OctoPrintVersionInfo? version = await client.GetFromJsonAsync<OctoPrintVersionInfo>("api/server");

            if (version == null)
            {
                throw new Exception("Connection to octoprint Failed");
            }
            
            status.info.Add("OctoPrint version", version.Version);
            if (version.Safemode != null)
            {
                status.info.Add("Safemode", version.Safemode);
            }
            #endregion Get Server Info

            OctoPrintConnectionInfo? connection = await client.GetFromJsonAsync<OctoPrintConnectionInfo>("api/connection");

            status.PrinterName = connection?.Options.PrinterProfiles.Where(pp => pp.Id == connection.Options.PrinterProfilePreference).Select(pp => pp.Name).FirstOrDefault() ?? "N/A";
            status.Status = connection?.Current.State ?? "N/A";
            status.Identifier = _options.Identifier;

            var job = await GetJobStatusAsync();

            status.addJobStatus(job);

            return status;
        }

        public async Task PauseAsync()
        {
            HttpClient client = _clientFactory.CreateAuthorizedClient(_options);

            var response = await client.SendOctoprintJobCommandAsync(new OctoprintActionCommand("pause","pause"));

            response.EnsureSuccessStatusCode();
        }

        public async Task StartAsync(string fileName)
        {
            HttpClient client = _clientFactory.CreateAuthorizedClient(_options);

            var CommandResponse = await client.SendOctoprintJobCommandAsync(new OctoprintFileCommand("select",fileName,true));

            CommandResponse.EnsureSuccessStatusCode();
        }

        public async Task StopAsync()
        {
            HttpClient client = _clientFactory.CreateAuthorizedClient(_options);

            var response = await client.SendOctoprintJobCommandAsync(new OctoprintCommand("cancel"));

            response.EnsureSuccessStatusCode();
        }

        public async Task UploadAsync(string FilePath)
        {
            HttpClient client = _clientFactory.CreateAuthorizedClient(_options);

            FileInfo file = new FileInfo(FilePath);
            FileStream fileStream = new FileStream(FilePath, FileMode.Open);

            var formData = new MultipartFormDataContent();
            formData.Add(new StreamContent(fileStream),"file",file.Name);

            var response = await client.PostAsync("/api/files/local", formData);

            response.EnsureSuccessStatusCode();
        }

        private int CalculateProgress(long time,long timeLeft)
        {
            
            if (timeLeft == 0)
            {
                return 0;
            }

            double proc = Convert.ToDouble(time) / 100;

            int procent = Convert.ToInt32((proc - 0.1 ) * timeLeft);

            return procent;
        }
    }
}

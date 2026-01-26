using _3DPrintLib.FlashForge.FlashDtos;
using Microsoft.Extensions.Options;
using PrintLib;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace _3DPrintLib.FlashForge
{
    public class FlashforgePrinter : IPrinter
    {
        private readonly IHttpClientFactory _ClientFactory;
        private readonly FlashforgeOptions _Options;

        private AuthTokenResponse AuthToken = null!; 

        public FlashforgePrinter(IHttpClientFactory clientFactory,FlashforgeOptions options)
        {
            _ClientFactory = clientFactory;
            _Options = options;
        }

        public async Task ContinueAsync()
        {
            using var client = await CreateAuthenticatedClientAsync();
            throw new NotImplementedException();
        }

        public async Task<JobStatus> GetJobStatusAsync()
        {
            using var client = await CreateAuthenticatedClientAsync();
            throw new NotImplementedException();
        }

        public async Task<PrinterStatus> GetStatusAsync()
        {
            using var client = await CreateAuthenticatedClientAsync();
            throw new NotImplementedException();
        }

        public async Task PauseAsync()
        {
            using var client = await CreateAuthenticatedClientAsync();
            throw new NotImplementedException();
        }

        public async Task StartAsync(string fileName)
        {
            using var client = await CreateAuthenticatedClientAsync();
            throw new NotImplementedException();
        }

        public async Task StopAsync()
        {
            using var client = await CreateAuthenticatedClientAsync();
            throw new NotImplementedException();
        }

        public async Task UploadAsync(string FilePath)
        {
            throw new NotImplementedException();
        }

        private async Task<HttpClient> CreateAuthenticatedClientAsync()
        {
            if (DateTime.UtcNow < DateTime.UtcNow.AddSeconds(AuthToken.ExpiresIn))
            {
                await AuthenticateAsync();
            }

            if (AuthToken == null)
            {
                await AuthenticateAsync();
            }

            HttpClient client = _ClientFactory.CreateClient();
            client.BaseAddress = new Uri(_Options.FullAddress);

            if (AuthToken != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken.Token);
            }
            
            return client;
        }

        private async Task AuthenticateAsync()
        {
            using HttpClient client = _ClientFactory.CreateClient();

            client.BaseAddress = new Uri(_Options.FullAddress);

            var payloadJson = JsonSerializer.Serialize(new { code = _Options.AccessCode });
            var content = new StringContent(payloadJson);            

            var result = await client.PostAsync("/api/auth/login",content);
            result.EnsureSuccessStatusCode();

            var response = await result.Content.ReadAsStringAsync();

            if (response != null)
            {
                var data = JsonSerializer.Deserialize<ApiResponse<AuthTokenResponse>>(response);

                if (data?.Success ?? true)
                {
                    throw new Exception("Authentication Failed.");
                }

                AuthToken = data.Data;
            }
        }
    }
}

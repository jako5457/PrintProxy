using 
    PrintLib.OctoPrint.OctoDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace PrintLib.OctoPrint
{
    public static class OctoPrintExtensions
    {

        internal static HttpClient CreateAuthorizedClient(this IHttpClientFactory factory, OctoPrintOptions options)
        {
            HttpClient client = factory.CreateClient();
            client.BaseAddress = options.EndPoint;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.ApiKey);

            return client;
        }

        internal static async Task<HttpResponseMessage> SendOctoprintJobCommandAsync<T>(this HttpClient client,T command) where T : OctoprintCommand
        {

            if (command is OctoprintFileCommand)
            {
                var cmd = command as OctoprintFileCommand;

                var response = await client.PostAsJsonAsync($"/api/files/local/{cmd?.File}", command);

                return response;
            }
            else
            {
                var response = await client.PostAsJsonAsync("/api/job", command);

                return response;
            }
        }

    }
}

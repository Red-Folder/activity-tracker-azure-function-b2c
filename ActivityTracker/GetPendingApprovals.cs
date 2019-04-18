using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ActivityTracker
{
    public static class GetPendingApprovals
    {
        private readonly static HttpClient _httpClient = new HttpClient();

        [FunctionName("GetPendingApprovals")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.User, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Received request to GetPendingApprovals");
            var url = Environment.GetEnvironmentVariable("GetPendingApprovalsUrl", EnvironmentVariableTarget.Process);

            log.Info($"Making request to {url}");
            var response = await _httpClient.GetAsync(url);

            log.Info($"Received status code {response.StatusCode}");

            return response;
        }
    }
}

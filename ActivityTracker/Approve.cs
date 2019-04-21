using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ActivityTracker
{
    public static class Approve
    {
        private readonly static HttpClient _httpClient = new HttpClient();

        [FunctionName("Approve")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req,
                                                            ClaimsPrincipal claimsPrincipal,
                                                            TraceWriter log)
        {
            log.Info("Received request to Approve");

            if (!claimsPrincipal.Identity.IsAuthenticated)
            {
                log.Info("Not authenticated");
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            ClaimsIdentity identity = (claimsPrincipal as ClaimsPrincipal)?.Identity as ClaimsIdentity;
            var name = identity.Claims.FirstOrDefault(x => x.Type == "name");

            var url = Environment.GetEnvironmentVariable("ApproveUrl", EnvironmentVariableTarget.Process);

            var requestBody = await req.Content.ReadAsStringAsync();
            var payload = new StringContent(requestBody, Encoding.UTF8, "application/json");

            log.Info($"Making request to {url} as {name}");
            var response = await _httpClient.PostAsync(url, payload);

            log.Info($"Received status code {response.StatusCode}");

            return response;
        }
    }
}

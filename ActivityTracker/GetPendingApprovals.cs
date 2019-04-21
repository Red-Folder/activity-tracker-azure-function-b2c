using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ActivityTracker
{
    public static class GetPendingApprovals
    {
        private readonly static HttpClient _httpClient = new HttpClient();

        [FunctionName("GetPendingApprovals")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req,
                                                            ClaimsPrincipal claimsPrincipal,
                                                            TraceWriter log)
        {
            log.Info("Received request to GetPendingApprovals");

            log.Info("Are we authenticated");
            if (!claimsPrincipal.Identity.IsAuthenticated)
            {
                log.Info("Not authenticated");
            }

            ClaimsIdentity identity = (claimsPrincipal as ClaimsPrincipal)?.Identity as ClaimsIdentity;
            foreach (var claim in identity.Claims)
            {
                log.Info($"{claim.Type} = {claim.Value}");
            }

            var url = Environment.GetEnvironmentVariable("GetPendingApprovalsUrl", EnvironmentVariableTarget.Process);

            log.Info($"Making request to {url}");
            var response = await _httpClient.GetAsync(url);

            log.Info($"Received status code {response.StatusCode}");

            return response;
        }
    }
}

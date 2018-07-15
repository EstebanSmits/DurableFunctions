using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions
{
    public static class ProcessStart
    {
        [FunctionName("ProcessStart")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, 
            [OrchestrationClient] DurableOrchestrationClient starter,
            TraceWriter log)
        {

            // parse query parameter
            string video = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => String.Compare(q.Key, "video", StringComparison.OrdinalIgnoreCase) == 0)
                .Value;
            dynamic data = await req.Content.ReadAsAsync<object>();
            video = video ?? data?.video;
            if (video == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please provide a video url in the request body");
            }

            log.Info($"Starting Orchestrator  for {video}");
            var orchestrationid = await starter.StartNewAsync("O_ProcessVideo", video);
            return starter.CreateCheckStatusResponse(req, orchestrationid);


        }
    }
}

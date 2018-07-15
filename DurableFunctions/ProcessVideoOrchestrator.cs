using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Threading.Tasks;

namespace DurableFunctions
{
    public static class ProcessVideoOrchestrator
    {
        [FunctionName("O_ProcessVideo")]
        public static async Task<object> ProcessVideo(
            [OrchestrationTrigger] DurableOrchestrationContext context, /* tells azure runtime that this is an orchestrator function */
            TraceWriter log 
            )
        {
            var videoLocation = context.GetInput<string>();
            if (!context.IsReplaying) { 
                log.Info("Start A_TranscodeVideo");
            }
            var transcodeLocation   = await context.CallActivityAsync<string>("A_TranscodeVideo", videoLocation); // will put process to sleep while it waits
            if (!context.IsReplaying)
            {
                log.Info("Start A_getThumbnail");
            }

            var thumbnailLocation =  await context.CallActivityAsync<string>("A_getThumbnail", transcodeLocation);
            if (!context.IsReplaying)
            {
                log.Info("Start A_prependIntro");
            }
            var introLocation = await context.CallActivityAsync<string>("A_prependIntro", transcodeLocation);
            return new {transcodeLocation, thumbnailLocation,introLocation};
        }
    }
}

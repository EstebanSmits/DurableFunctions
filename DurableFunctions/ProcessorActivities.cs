using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctions
{
    public static class ProcessorActivities
    {
        [FunctionName("A_TranscodeVideo")]
        public static async Task<string> TranscodeVideo( [ActivityTrigger] string inputVideo, TraceWriter log)
        {
            log.Info($" Transcoding {inputVideo}" );
            await Task.Delay(5000);
            return "transcoded.mp4";
        }
        [FunctionName("A_getThumbnail")]
        public static async Task<string> GetThumbnail([ActivityTrigger] string transcoded, TraceWriter log)
        {
            log.Info($" generating Thumbnail for {transcoded}");
            await Task.Delay(5000);
            return "thumbnail.jpg";
        }
        [FunctionName("A_prependIntro")]
        public static async Task<string> PrependIntro([ActivityTrigger] string transcoded, TraceWriter log)
        {
            log.Info($" generating prepended intro for {transcoded}");
            await Task.Delay(5000);
            return "prependedvideo.mp4";
        }

    }
}

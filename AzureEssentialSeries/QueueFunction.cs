using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureEssentialSeries
{
    public class QueueFunction
    {
        [FunctionName("QueueFunction")]
        public void Run([QueueTrigger("myqueue-items", Connection = "http://127.0.0.1:10001")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Queues.Models;
using E2.Models;
using Microsoft.Azure.Functions.Worker.Http;

namespace E2
{
    public class SendMessage
    {
        private readonly ILogger<SendMessage> _logger;

        public SendMessage(ILogger<SendMessage> logger)
        {
            _logger = logger;
        }

        [Function("SendMessage")]
        [QueueOutput("messages",Connection ="dev:queue")]
        public MessageDto Run([HttpTrigger(AuthorizationLevel.Anonymous,"post")]HttpRequestData req, [FromBody]MessageDto message)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return message;
        }
    }
}

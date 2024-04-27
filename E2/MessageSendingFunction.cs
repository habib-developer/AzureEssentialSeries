using System;
using Azure.Storage.Queues.Models;
using E2.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace E2
{
    public class MessageSendingFunction
    {
        private readonly ILogger<MessageSendingFunction> _logger;

        public MessageSendingFunction(ILogger<MessageSendingFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(MessageSendingFunction))]
        public void Run([QueueTrigger("messages", Connection = "dev:queue")] MessageDto message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.Body}");
            // Write your code to send messages to users
            
        }
    }
}

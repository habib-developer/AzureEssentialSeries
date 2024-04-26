using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace E1_AzureFunctions
{
    public class QueueFunction
    {
        private readonly ILogger<QueueFunction> _logger;

        public QueueFunction(ILogger<QueueFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(QueueFunction))]
        public void Run([QueueTrigger("student-items", Connection = "dev")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}

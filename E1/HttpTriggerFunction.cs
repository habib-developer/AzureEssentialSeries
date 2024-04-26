using E1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace E1
{
    public class HttpTriggerFunction
    {
        private readonly ILogger<HttpTriggerFunction> _logger;
        private readonly IStudentService _studentService;
        public HttpTriggerFunction(ILogger<HttpTriggerFunction> logger, IStudentService studentService)
        {
            _logger = logger;
            _studentService = studentService;
        }

        [Function("GetAllStudents")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var result = _studentService.GetAllStudents();
            return new OkObjectResult(result);
        }
    }
}

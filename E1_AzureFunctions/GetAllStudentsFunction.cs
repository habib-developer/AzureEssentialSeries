using E1_AzureFunctions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace E1_AzureFunctions
{
    public class GetAllStudentsFunction
    {
        private readonly ILogger<GetAllStudentsFunction> _logger;
        private readonly IStudentService _studentService;
        public GetAllStudentsFunction(ILogger<GetAllStudentsFunction> logger,IStudentService studentService)
        {
            _logger = logger;
            _studentService = studentService;
        }

        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var result = _studentService.GetAllStudents();
            return new OkObjectResult(result);
        }
    }
}

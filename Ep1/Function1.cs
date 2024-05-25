using Ep1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ep1
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("InsertStudent")]
        [TableOutput("studentsTable", "AzureWebJobsStorage")]
        public StudentEntity InsertStudent([HttpTrigger(AuthorizationLevel.Function,"post")] HttpRequest req, [Microsoft.Azure.Functions.Worker.Http.FromBody]StudentEntity student)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            student.RowKey = Guid.NewGuid().ToString();
            return student;
        }
        [Function("GetAllMaleStudents")]
        public IActionResult GetAllMaleStudents([HttpTrigger(AuthorizationLevel.Function,"get")] HttpRequest req,
            [TableInput("studentsTable","male",Connection = "AzureWebJobsStorage")] IEnumerable<StudentEntity> students)
        {
            return new OkObjectResult(students);
        }
    }
}

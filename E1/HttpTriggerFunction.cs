using Azure;
using Azure.Data.Tables;
using E1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
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
        public IActionResult GetAllStudents([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [TableInput("emails","fail",2,Connection ="storage")] IEnumerable<Student> students)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var result = _studentService.GetAllStudents();
            TableClient client = new TableClient("UseDevelopmentStorage=true", "emails");
            var entity =client.GetEntity<Student>("fail", "d1ea6d91-8ee6-8789-abb1-3da493e59fb5");
            if(entity != null)
            {
                entity.Value.Name = "Updated from Hello world";
                client.UpdateEntity<Student>(entity,ETag.All);
            }
            return new OkObjectResult(students);
        }
        [Function("InsertStudent")]
        [TableOutput("emails", "storage")]
        public Student InsertRun([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")][Microsoft.Azure.Functions.Worker.Http.FromBody]Student req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return req;
        }
    }
    public class Student : ITableEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}

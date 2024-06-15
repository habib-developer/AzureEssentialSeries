using Dapper;
using Ep4.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Ep4
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly IConfiguration _configuration;

        public Function1(ILogger<Function1> logger,IConfiguration configuration)
        {
            _logger = logger;
            this._configuration = configuration;
        }
        [Function("GetAll")]
        public async Task<IActionResult> GetAllAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            [SqlInput("select * from dbo.Employees", "ConnectionStrings:aeslearn")] IEnumerable<Employee> employees)
        {
            return new OkObjectResult(employees);
        }
        [Function("Create")]
        public async Task<UpsertEmployeeResponse> CreateAsync([HttpTrigger(AuthorizationLevel.Anonymous,"post")] HttpRequestData req,[Microsoft.Azure.Functions.Worker.Http.FromBody] Employee model)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync("Employee created successfully!");

            // Return a response to both HTTP trigger and Azure SQL output binding.
            return new UpsertEmployeeResponse()
            {
                Employee = new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Email = model.Email
                },
                HttpResponse = response
            };
        }
        [Function("Update")]
        public async Task<UpsertEmployeeResponse> UpdateAsync([HttpTrigger(AuthorizationLevel.Anonymous,"put")] HttpRequestData req, [Microsoft.Azure.Functions.Worker.Http.FromBody] Employee model)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync("Employee updated successfully!");

            // Return a response to both HTTP trigger and Azure SQL output binding.
            return new UpsertEmployeeResponse()
            {
                Employee = new Employee
                {
                    Id = model.Id,
                    Name = model.Name,
                    Email = model.Email
                },
                HttpResponse = response
            };
        }
        [Function("Delete")]
        public async Task<IActionResult> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Delete/{id}")] HttpRequest request,Guid id)
        {
            var connectionString = _configuration["ConnectionStrings:aeslearn"];
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("delete from dbo.Employees where Id=@Id", new { Id = id });
            return new OkObjectResult("Deleted successfully");
        }
    }
}

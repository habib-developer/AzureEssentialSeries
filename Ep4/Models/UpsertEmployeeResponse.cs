using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ep4.Models
{
    public class UpsertEmployeeResponse
    {
        [SqlOutput("dbo.Employees", connectionStringSetting: "ConnectionStrings:aeslearn")]
        public Employee Employee { get; set; }
        public HttpResponseData HttpResponse { get; set; }
    }
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}

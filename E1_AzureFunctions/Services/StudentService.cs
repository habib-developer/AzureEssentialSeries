using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E1_AzureFunctions.Services
{
    internal class StudentService : IStudentService
    {
        public IEnumerable<string> GetAllStudents()
        {
            return new List<string>()
            {
                "Habib",
                "Jabir",
                "Jack",
                "Umer"
            };
        }
    }
}

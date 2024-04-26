using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E1.Services
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

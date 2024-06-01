using Ep2.Models;
using Ep2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ep2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController(StudentService studentService) : ControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await studentService.GetAll();
            return Ok(response);
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await studentService.Get(id);
            return Ok(response);
        }
        [HttpPut("Insert")]
        public async Task<IActionResult> Insert(Student request)
        {
            await studentService.Insert(request);
            return Ok("Student created successfully");
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            await studentService.Delete(id);
            return Ok("Student delete successfully");
        }
    }
}

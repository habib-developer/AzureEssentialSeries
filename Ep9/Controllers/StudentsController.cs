using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ep9.Data;
using Ep9.Models;
using Ep9.Services;

namespace Ep9.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;

        public StudentsController(ApplicationDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var cachedStudents = await _cacheService.GetAsync<List<Student>>("students");
            if(cachedStudents is not null)
            {
                return View(cachedStudents);
            }
            var databaseStudents = await _context.Students.ToListAsync();
            await _cacheService.SetAsync<List<Student>>("students", databaseStudents);
            return View(databaseStudents);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //get from azure cache
            var cachedStudent = await _cacheService.GetAsync<Student>($"student_{id}");
            if(cachedStudent is not null)
            {
                return View(cachedStudent);
            }
            //get from database
            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            await _cacheService.SetAsync<Student>($"student_{id}", student);
            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,PhoneNo,Address,City,State,Country,CreatedAt,UpdatedAt")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.Id = Guid.NewGuid();
                student.CreatedAt = student.UpdatedAt = DateTime.UtcNow;
                _context.Add(student);
                await _context.SaveChangesAsync();
                await _cacheService.SetAsync<Student>($"student_{student.Id}", student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Email,PhoneNo,Address,City,State,Country,CreatedAt,UpdatedAt")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    student.UpdatedAt = DateTime.UtcNow;
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    await _cacheService.SetAsync<Student>($"student_{student.Id}",student);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            await _cacheService.RemoveAsync($"student_{id}");
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}

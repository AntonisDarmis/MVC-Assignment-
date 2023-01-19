using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversitySystemWeb.Models;

namespace UniversitySystemWeb.Controllers
{
    public class CourseHasStudentsController : Controller
    {
        private readonly GraderDBContext _context;

        public CourseHasStudentsController(GraderDBContext context)
        {
            _context = context;
        }

        // GET: CourseHasStudents
        public async Task<IActionResult> Index()
        {
       
            return View();
        }

        

       


        // GET: CourseHasStudents/Edit/5
        public async Task<IActionResult> InsertGrade(int courseId,int professorId,string courseName)
        {
            ViewBag.name = courseName;
            if (courseId == null || _context.CourseHasStudents == null)
            {
                return NotFound();
            }

            var courseHasStudent = await _context.CourseHasStudents.FindAsync(courseId);
            if (courseHasStudent == null)
            {
                return NotFound();
            }
            ViewData["CourseIdCourse"] = new SelectList(_context.Courses, "IdCourse", "IdCourse", courseHasStudent.CourseIdCourse);
            ViewData["StudentsRegistrationNumber"] = new SelectList(_context.Students, "RegistrationNumber", "RegistrationNumber", courseHasStudent.StudentsRegistrationNumber);
            return View(courseHasStudent);
        }





        // POST: CourseHasStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertGrade(int courseId,int professorId,[Bind("CourseIdCourse,StudentsRegistrationNumber,GradeCourseStudent")] CourseHasStudent courseHasStudent)
        {
            if (courseHasStudent.CourseIdCourse==0)
            {
                return NotFound();
            }
            var errors = ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => new { x.Key, x.Value.Errors })
            .ToArray();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseHasStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseHasStudentExists(courseHasStudent.CourseIdCourse))
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
            ViewData["CourseIdCourse"] = new SelectList(_context.Courses, "IdCourse", "IdCourse", courseHasStudent.CourseIdCourse);
            ViewData["StudentsRegistrationNumber"] = new SelectList(_context.Students, "RegistrationNumber", "RegistrationNumber", courseHasStudent.StudentsRegistrationNumber);
            return RedirectToAction("Index", "Professor", professorId);
        }

       

        // POST: CourseHasStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CourseHasStudents == null)
            {
                return Problem("Entity set 'GraderDBContext.CourseHasStudents'  is null.");
            }
            var courseHasStudent = await _context.CourseHasStudents.FindAsync(id);
            if (courseHasStudent != null)
            {
                _context.CourseHasStudents.Remove(courseHasStudent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseHasStudentExists(int id)
        {
          return _context.CourseHasStudents.Any(e => e.CourseIdCourse == id);
        }
    }
}

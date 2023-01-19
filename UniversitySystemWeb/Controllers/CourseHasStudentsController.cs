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
        public async Task<IActionResult> InsertGrade(int courseId,int professorId,int registrationNumber,string courseName)
        {
            ViewBag.courseId = courseId;
            ViewBag.professorId = professorId;
            ViewBag.regNumber = registrationNumber;
            ViewBag.name = courseName;
            if (courseId == null || _context.CourseHasStudents == null)
            {
                return NotFound();
            }

            var courseHasStudent = await _context.CourseHasStudents.FindAsync(courseId,registrationNumber);
            if (courseHasStudent == null)
            {
                return NotFound();
            }
           
            return View(courseHasStudent);
        }





        // POST: CourseHasStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertGrade(int courseId,int professorId, int registrationNumber, [Bind("CourseIdCourse,StudentsRegistrationNumber,GradeCourseStudent")] CourseHasStudent courseHasStudent)
        {
            if (courseHasStudent.CourseIdCourse==0)
            {
                return NotFound();
            }
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
            
            return RedirectToAction("ViewGrade", "Professors", new {id=professorId});
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

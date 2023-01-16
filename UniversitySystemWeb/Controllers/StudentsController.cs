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
    public class StudentsController : Controller
    {
        private readonly GraderDBContext _context;

        public StudentsController(GraderDBContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string username)
        {
            var student = await _context.Students
                .Include(s => s.UsersUsernameNavigation)
                .FirstOrDefaultAsync();
            if (student!=null) 
            {
                return View(student);
            }
            return NotFound(); 
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.UsersUsernameNavigation)
                .FirstOrDefaultAsync(m => m.RegistrationNumber == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }



        public async Task<IActionResult> GradePerCourse(int? id)
        {
            var student = await _context.Students
                .Include(s => s.UsersUsernameNavigation)
                .FirstOrDefaultAsync(m => m.RegistrationNumber == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GradePerCourse(int? id,string courseName)
        {
            var student = await _context.Students
                .Include(s => s.UsersUsernameNavigation)
                .FirstOrDefaultAsync(m => m.RegistrationNumber == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }




        public async Task<IActionResult> GradePerSemester(int? id)
        {
            var student = await _context.Students
                 .Include(s => s.UsersUsernameNavigation)
                 .FirstOrDefaultAsync(m => m.RegistrationNumber == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

       
        public async Task<IActionResult> TotalGrade(int? id)
        {
            var student = await _context.Students
                 .Include(s => s.UsersUsernameNavigation)
                 .FirstOrDefaultAsync(m => m.RegistrationNumber == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        
        

      
        private bool StudentExists(int id)
        {
          return _context.Students.Any(e => e.RegistrationNumber == id);
        }
    }
}

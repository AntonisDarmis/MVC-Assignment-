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
    public class ProfessorsController : Controller
    {
        private readonly GraderDBContext _context;

        public ProfessorsController(GraderDBContext context)
        {
            _context = context;
        }

        // GET: Professors
        public async Task<IActionResult> Index(string username)
        {
            var professor = await _context.Professors
                .Include(s => s.UsersUsernameNavigation)
                .FirstOrDefaultAsync(m=> m.UsersUsername==username);
            ViewBag.id = professor.Afm;
            if (professor != null)
            {
                return View(professor);
            }
            return NotFound();
        }

        



    

        public async Task<IActionResult> ViewGrade(int? id,string cTitle = "") 
        {
            
            ViewBag.id = id;
            var courses = (from course in _context.Courses
                          join courseGrades in _context.CourseHasStudents on course.IdCourse equals courseGrades.CourseIdCourse
                          into result
                          from item in result
                          join prof in _context.Professors on course.ProfessorsAfm equals prof.Afm
                          where item.GradeCourseStudent != null && course.ProfessorsAfm==id
                          select new ViewModel
                          { grade = (int)item.GradeCourseStudent, title = course.CourseTitle, semester = course.CourseSemester, registrationNumber = (int)item.StudentsRegistrationNumber }).OrderBy(x=>x.semester);

            var titles = new List<string>(courses.Select(x => x.title).Distinct());
            List<SelectListItem> courseTitles  = titles.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.ToString(),
                    Value = a.ToString(),
                    Selected = false
                };
            });
            
            ViewBag.cTitle = cTitle;

            ViewBag.courseTitles = courseTitles;
            if (courses != null) 
            {
                return View(courses);
            }             
            return View();
        }

        public async Task<IActionResult> ViewNotGraded(int? id)
        {
           
            ViewBag.id = id;
            var courses = (from course in _context.Courses
                          join courseGrades in _context.CourseHasStudents on course.IdCourse equals courseGrades.CourseIdCourse
                          into result
                          from item in result
                          join prof in _context.Professors on course.ProfessorsAfm equals prof.Afm
                          where item.GradeCourseStudent == null && course.ProfessorsAfm == id
                           select new ViewModel
                          { title = course.CourseTitle, semester = course.CourseSemester, registrationNumber = (int)item.StudentsRegistrationNumber,professorId = (int)id, courseId=item.CourseIdCourse }).OrderBy(x=>x.semester);
            
            if (courses != null)
            {
                return View(courses);
            }
            return View();
        }

        public IActionResult Create(string username)
        {
            ViewData["UsersUsername"] = new SelectList(_context.Users, "Username", "Username");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string username,[Bind("Afm,Name,Surname,Department,UsersUsername")] Professor professor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(professor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Secretaries", new { username = username });
            }
            return NotFound();
            
        }




        private bool ProfessorExists(int id)
        {
          return _context.Professors.Any(e => e.Afm == id);
        }
        private bool CourseHasStudentExists(int id)
        {
            return _context.CourseHasStudents.Any(e => e.CourseIdCourse == id);
        }
    }
}

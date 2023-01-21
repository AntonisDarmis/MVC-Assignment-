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
                .FirstOrDefaultAsync(m=> m.UsersUsername==username);
            ViewBag.id = student.RegistrationNumber;
            if (student!=null) 
            {
                return View(student);
            }
            return NotFound(); 
        }

       


        public async Task<IActionResult> GradePerCourse(int? id)
        {
            var student = await _context.Students
               .Include(s => s.UsersUsernameNavigation)
               .FirstOrDefaultAsync(m => m.RegistrationNumber == id);
            var grade = from stud in _context.Students
                         join courseGrades in _context.CourseHasStudents on stud.RegistrationNumber equals courseGrades.StudentsRegistrationNumber
                         into res1
                         from item in res1
                         join course in _context.Courses on item.CourseIdCourse equals course.IdCourse
                         where stud.RegistrationNumber == id && item.GradeCourseStudent != null
                        select new ViewModel
                         {grade = (int)item.GradeCourseStudent, title = course.CourseTitle, semester = course.CourseSemester };
            ViewBag.id = student.RegistrationNumber;
            if (grade != null)
            {
                return View(grade);
            }
            return View();
        }

        
        public async Task<IActionResult> GradePerSemester(int? id,string semester="1")
        {

            var student = await _context.Students
                 .Include(s => s.UsersUsernameNavigation)
                 .FirstOrDefaultAsync(m => m.RegistrationNumber == id);
            ViewBag.id = student.RegistrationNumber;
            if (student == null)
            {
                return NotFound();
            }
            var courses = from stud in _context.Students
                          join courseGrades in _context.CourseHasStudents on stud.RegistrationNumber equals courseGrades.StudentsRegistrationNumber
                          into res1
                          from item in res1
                          join course in _context.Courses on item.CourseIdCourse equals course.IdCourse
                          where stud.RegistrationNumber == id && course.CourseSemester == semester && item.GradeCourseStudent != null
                          select new ViewModel
                          { grade = (int)item.GradeCourseStudent, title = course.CourseTitle, semester = course.CourseSemester, registrationNumber = (int)item.StudentsRegistrationNumber };
            ViewBag.id = student.RegistrationNumber;

            if (courses != null) 
            {
                return View(courses);
            }
            return View();
        }






    

       
        public async Task<IActionResult> TotalGrade(int? id)
        {
            
            var student = await _context.Students
                 .Include(s => s.UsersUsernameNavigation)
                 .FirstOrDefaultAsync(m => m.RegistrationNumber == id);
            ViewBag.id = student.RegistrationNumber;
            var courses = from stud in _context.Students
                          join courseGrades in _context.CourseHasStudents on stud.RegistrationNumber equals courseGrades.StudentsRegistrationNumber
                          into res1
                          from item in res1
                          join course in _context.Courses on item.CourseIdCourse equals course.IdCourse
                          where stud.RegistrationNumber == id && item.GradeCourseStudent != null
                          select new ViewModel
                          { grade = (int)item.GradeCourseStudent, title = course.CourseTitle, semester = course.CourseSemester };
            if (student == null)
            {
                return NotFound();
            }
            int totalGrade = 0;
            foreach (var item in courses) 
            {
                totalGrade += item.grade;
            }
            if (courses.Count() > 0)
            {
                totalGrade = totalGrade / courses.Count();
            }
            return View(ViewBag.Total = totalGrade);
        }

        
        

      
        private bool StudentExists(int id)
        {
          return _context.Students.Any(e => e.RegistrationNumber == id);
        }
    }
}

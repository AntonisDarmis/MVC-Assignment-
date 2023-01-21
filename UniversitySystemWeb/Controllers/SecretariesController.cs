using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversitySystemWeb.Models;

namespace UniversitySystemWeb.Controllers
{
    public class SecretariesController : Controller
    {
        private readonly GraderDBContext _context;

        public SecretariesController(GraderDBContext context)
        {
            _context = context;
        }

        // GET: Secretaries
        public async Task<IActionResult> Index()
        {
            //ViewBag.username = username;
            var graderDBContext = _context.Secretaries.Include(s => s.UsersUsernameNavigation);
            return View(await graderDBContext.ToListAsync());
        }

        // GET: Secretaries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Secretaries == null)
            {
                return NotFound();
            }

            var secretary = await _context.Secretaries
                .Include(s => s.UsersUsernameNavigation)
                .FirstOrDefaultAsync(m => m.Phonenumber == id);
            if (secretary == null)
            {
                return NotFound();
            }

            return View(secretary);
        }

        // GET: Secretaries/Create
        public IActionResult Create()
        {
            ViewData["UsersUsername"] = new SelectList(_context.Users, "Username", "Username");
            return View();
        }

        // POST: Secretaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Phonenumber,Name,Surname,Department,UsersUsername")] Secretary secretary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(secretary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsersUsername"] = new SelectList(_context.Users, "Username", "Username", secretary.UsersUsername);
            return View(secretary);
        }

        // GET: Secretaries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Secretaries == null)
            {
                return NotFound();
            }

            var secretary = await _context.Secretaries.FindAsync(id);
            if (secretary == null)
            {
                return NotFound();
            }
            ViewData["UsersUsername"] = new SelectList(_context.Users, "Username", "Username", secretary.UsersUsername);
            return View(secretary);
        }

        // POST: Secretaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Phonenumber,Name,Surname,Department,UsersUsername")] Secretary secretary)
        {
            if (id != secretary.Phonenumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(secretary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecretaryExists(secretary.Phonenumber))
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
            ViewData["UsersUsername"] = new SelectList(_context.Users, "Username", "Username", secretary.UsersUsername);
            return View(secretary);
        }

        // GET: Secretaries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Secretaries == null)
            {
                return NotFound();
            }

            var secretary = await _context.Secretaries
                .Include(s => s.UsersUsernameNavigation)
                .FirstOrDefaultAsync(m => m.Phonenumber == id);
            if (secretary == null)
            {
                return NotFound();
            }

            return View(secretary);
        }

        // POST: Secretaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Secretaries == null)
            {
                return Problem("Entity set 'GraderDBContext.Secretaries'  is null.");
            }
            var secretary = await _context.Secretaries.FindAsync(id);
            if (secretary != null)
            {
                _context.Secretaries.Remove(secretary);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> InsertCourse()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertCourse([Bind("IdCourse,CourseTitle,CourseSemester,ProfessorsAfm")] Course course)
        {
            _context.Add(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Secretaries");
        }

        // GET: Secretaries/InsertProfessor
        public async Task<IActionResult> InsertProfessor()
        {
            return View();
        }
        

        // POST: Secretaries/InsertProfessor
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertProfessor([Bind("username,password,role,afm,name,surname,department")] UserProfessor userProfessor)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.Username = userProfessor.username;
                user.Password = userProfessor.password;
                user.Role = userProfessor.role;
                _context.Add(user);
                await _context.SaveChangesAsync();

                Professor professor = new Professor();
                professor.Afm = userProfessor.afm;
                professor.Name = userProfessor.name;
                professor.Surname = userProfessor.surname;
                professor.Department = userProfessor.department;
                professor.UsersUsername = userProfessor.username;

                _context.Add(professor);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index","Secretaries");
            }
            return View();
        }



        public async Task<IActionResult> InsertStudent() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertStudent([Bind("username,password,role,registrationnumber,name,surname,department")] UserStudent userStudent)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.Username = userStudent.username;
                user.Password = userStudent.password;
                user.Role = userStudent.role;
                _context.Add(user);
                await _context.SaveChangesAsync();

                Student student = new Student();
                student.RegistrationNumber = userStudent.registrationnumber;
                student.Name = userStudent.name;
                student.Surname = userStudent.surname;
                student.Department = userStudent.department;
                student.UsersUsername = userStudent.username;

                _context.Add(student);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Secretaries");
            }
            return View();
        }

        

        public async Task<IActionResult> ViewCourses() 
        {
            var courses = (from course in _context.Courses 
                           select new ViewModel { courseId = course.IdCourse,title = course.CourseTitle, semester = course.CourseSemester, professorId = course.ProfessorsAfm}
                           ).OrderBy(x => x.semester);
            if (courses!=null)
            {
                return View(courses);
            }
            return View();
        }

        public async Task<IActionResult> AssignCourse(int courseId, int profAfm, string semester,string courseTitle) 
        {
            ViewBag.courseId = courseId;
            ViewBag.profAfm = profAfm;
            ViewBag.courseTitle = courseTitle;
            ViewBag.semester = semester;
            if (courseId == null || profAfm==null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(courseId);
            if(course==null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignCourse(int courseId, int profAfm, string semester,string courseTitle, [Bind("IdCourse,CourseTitle,CourseSemester,ProfessorsAfm")] Course course)
        {
            if(course.IdCourse==0)
            {
                return NotFound();
            }

            try
            {
                _context.Update(course);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction("Index","Secretaries");
        }



        public async Task<IActionResult> RegisterCourse(int courseId, string courseTitle) 
        {
            ViewBag.courseId = courseId;
            ViewBag.courseTitle = courseTitle;
            if (courseId == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(courseId);
            if (course==null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCourse(int courseId,string courseTitle, [Bind("CourseIdCourse,StudentsRegistrationNumber,GradeCourseStudent")] CourseHasStudent courseHasStudent)
        {
            if (courseHasStudent == null)
            {
                return NotFound();
            }
            try
            {
                var found = await _context.CourseHasStudents.FindAsync(courseHasStudent.CourseIdCourse, courseHasStudent.StudentsRegistrationNumber);
                if (found == null)
                {
                    _context.Add(courseHasStudent);
                    await _context.SaveChangesAsync();
                }
            } catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction("Index", "Secretaries");
        }















        private bool SecretaryExists(int id)
        {
          return _context.Secretaries.Any(e => e.Phonenumber == id);
        }
    }
}

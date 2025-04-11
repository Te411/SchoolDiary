using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTO.Student;
using MyWebApplicationServer.DTO.User;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Controllers
{
    [Route("api/Student")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public StudentsController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Students
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDtoForRole>>> GetStudent()
        {

            return await _context.Student
                .Include(s => s.Class)
                .Include(s => s.User)
                .Select(s => new StudentDtoForRole
                {
                    StudentId = s.StudentId,
                    ClassId = s.ClassId,
                    UserId = s.UserId,
                    Class = new ClassDto { Name = s.Class.Name },
                    User = new UserDtoForRole
                    {
                        Email = s.User.Email,
                        Password = s.User.Password,
                        Name = s.User.Name,
                        Login = s.User.Login,
                        InActive = s.User.InActive
                    }
                })
                .ToListAsync();
        }

        /// <summary>
        /// GET: api/Students/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(Guid id)
        {
            var student = await _context.Student.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        /// <summary>
        /// POST: api/Students
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Student.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.StudentId }, student);
        }

        /// <summary>
        /// DELETE: api/Students/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Student.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

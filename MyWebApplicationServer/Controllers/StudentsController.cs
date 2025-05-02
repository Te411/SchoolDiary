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
        /// Получить всех студентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentForRoleDto>>> GetStudent()
        {

            return await _context.Student
                .Include(s => s.Class)
                .Include(s => s.User)
                .Select(s => new StudentForRoleDto
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
        /// Получить студента по его id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(Guid id)
        {
            var student = await _context.Student.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        /// <summary>
        /// Получить студента по User id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("userId/{id}")]
        public async Task<ActionResult<IEnumerable<StudentForGradeDto>>> GetStudentByUserId(Guid id)
        {
            var student = await _context.Student
                .Where(s => s.UserId == id)
                .Include(s => s.User)
                .Select(s => new StudentForGradeDto
                {
                    StudentId = s.StudentId,
                    Name = s.User.Name
                })
                .ToListAsync();

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        /// <summary>
        /// Получить студента по названию класса
        /// </summary>
        /// <param name="сlassName"></param>
        /// <returns></returns>
        [HttpGet("ClassName/{сlassName}")]
        public async Task<ActionResult<IEnumerable<StudentForGradeDto>>> GetStudentByUserId(string сlassName)
        {
            var student = await _context.Student
                .Where(s => s.Class.Name == сlassName)
                .Include(s => s.User)
                .Select(s => new StudentForGradeDto
                {
                    StudentId = s.StudentId,
                    Name = s.User.Name
                })
                .ToListAsync();

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        /// <summary>
        /// Добавить нового студента
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
        /// Удалить студента
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

        /// <summary>
        /// Получить общую информацию о студенте
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GeneralInfo/{id}")]
        public async Task<ActionResult<IEnumerable<StudentGeneralInfoDto>>> GetGeneralInfoStudent(Guid id)
        {
            var student = await _context.Student
                .Where(s => s.StudentId == id)
                .Include(s => s.User)
                .Include(s => s.Class)
                .Select(s => new StudentGeneralInfoDto
                {
                    Name = s.User.Name,
                    Email = s.User.Email,
                    ClassName = s.Class.Name,
                })
                .ToListAsync();

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }
    }
}

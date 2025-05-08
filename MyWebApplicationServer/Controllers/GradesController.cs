using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTOs.Grade;
using MyWebApplicationServer.DTOs.Student;
using MyWebApplicationServer.DTOs.Subject;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Controllers
{
    /// <summary>
    /// Контроллер для таблицы "Оценка"
    /// </summary>
    [Route("api/Grade")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly LibraryContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public GradesController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить все существующие оценки
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GradeDto>>> GetGrade()
        {
            return await _context.Grade
                .Include(g => g.Subject)
                .Include(g => g.Student)
                    .ThenInclude(s => s.User)
                .Select(g => new GradeDto
                {
                    GradeId = g.GradeId,
                    SubjectId = g.SubjectId,
                    SubjectName = g.Subject.Name,
                    Value = g.Value,
                    Data = g.Data,
                    Student = new StudentForGradeDto
                    {
                        StudentId = g.Student.StudentId,
                        Name = g.Student.User.Name,
                    }
                })
                .ToListAsync();
        }

        /// <summary>
        /// Получить оценку по id
        /// </summary>
        /// <param name="id">id Оценки</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("id/{id}")]
        public async Task<ActionResult<IEnumerable<GradeDto>>> GetGrade(Guid id)
        {
            var grade = await _context.Grade
                .Where(g=> g.GradeId == id)
                .Include(g => g.Subject)
                .Include(g => g.Student)
                    .ThenInclude(s => s.User)
                .Select(g => new GradeDto
                {
                    GradeId = g.GradeId,
                    SubjectId = g.SubjectId,
                    SubjectName = g.Subject.Name,
                    Value = g.Value,
                    Data = g.Data,
                    Student = new StudentForGradeDto
                    {
                        StudentId = g.Student.StudentId,
                        Name = g.Student.User.Name,
                    }
                })
                .ToListAsync();

            if (grade == null)
            {
                return NotFound();
            }

            return grade;
        }

        /// <summary>
        /// Получить все оценки по названию предмета
        /// </summary>
        /// <param name="subjectName">Название предмета</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("subjectName/{subjectName}")]
        public async Task<ActionResult<IEnumerable<GradeDto>>> GetGrade(string subjectName)
        {
            var grade = await _context.Grade
                .Where(g => g.Subject.Name == subjectName)
                .Include(g => g.Subject)
                .Include(g => g.Student)
                    .ThenInclude(s => s.User)
                .Select(g => new GradeDto
                {
                    GradeId = g.GradeId,
                    SubjectId = g.SubjectId,
                    SubjectName = g.Subject.Name,
                    Value = g.Value,
                    Data = g.Data,
                    Student = new StudentForGradeDto
                    {
                        StudentId = g.Student.StudentId,
                        Name = g.Student.User.Name,
                    }
                })
                .ToListAsync();

            if (grade == null)
            {
                return NotFound();
            }

            return grade;
        }

        /// <summary>
        /// Добавление новой оценки
        /// </summary>
        /// <param name="createGradeDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Учитель")]
        [HttpPost]
        public async Task<ActionResult<GradeDto>> PostGrade(CreateGradeDto createGradeDto)
        {
            var subjectExists = await _context.Subject
                .FirstOrDefaultAsync(s => s.Name == createGradeDto.SubjectName);

            if (subjectExists == null)
            {
                return BadRequest("Указанный предмет не существует");
            }

            var studentExists = await _context.Student
                .FirstOrDefaultAsync(s => s.User.Name == createGradeDto.StudentName);

            if (studentExists == null)
            {
                return BadRequest("Указанный студент не существует");
            }

            var grade = new Grade
            {
                SubjectId = subjectExists.SubjectId,
                StudentId = studentExists.StudentId,
                Value = createGradeDto.Value,
                Data = createGradeDto.Data
            };

            _context.Grade.Add(grade);
            await _context.SaveChangesAsync();

            var createdGrade = await _context.Grade
                .Where(g => g.GradeId == grade.GradeId)
                .Include(g => g.Subject)
                .Include(g => g.Student)
                    .ThenInclude(s => s.User)
                .Select(g => new GradeDto
                {
                    GradeId = g.GradeId,
                    SubjectId = g.SubjectId,
                    SubjectName = g.Subject.Name,
                    Value = g.Value,
                    Data = g.Data,
                    Student = new StudentForGradeDto
                    {
                        StudentId = g.Student.StudentId,
                        Name = g.Student.User.Name,
                    },
                })
                .FirstOrDefaultAsync();

            return CreatedAtAction("GetGrade", new { id = grade.GradeId }, createdGrade);
        }

        /// <summary>
        /// Получить все оценки по названию предмета и класса
        /// </summary>
        /// <param name="gradeAllStudent"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GradeAllStudentByClassSubject/")]
        public async Task<ActionResult<IEnumerable<GradeDto>>> GetGradeAllStudentByClassSubject(GradeByClassSubjectDto gradeAllStudent)
        {
            var grade = await _context.Grade
                .Where(g => g.SubjectId == gradeAllStudent.SubjectId && g.Student.ClassId == gradeAllStudent.ClassId)
                .Include(g => g.Student)
                    .ThenInclude(s => s.User)
                .Include(g => g.Subject)
                .Select(g => new GradeDto
                {
                    GradeId = g.GradeId,
                    SubjectId = g.SubjectId,
                    SubjectName = g.Subject.Name,
                    Value = g.Value,
                    Data = g.Data,
                    Student = new StudentForGradeDto
                    {
                        StudentId = g.Student.StudentId,
                        Name = g.Student.User.Name,
                    }
                })
                .ToListAsync();

            if (grade == null)
            {
                return NotFound();
            }

            return grade;
        }

        /// <summary>
        /// Получить все оценки по id студента
        /// </summary>
        /// <param name="StudentId">id студента</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("StudentId/{StudentId}")]
        public async Task<ActionResult<IEnumerable<SubjectForGradeDto>>> GetGradeByStudentId(Guid StudentId)
        {
            var grade = await _context.Grade
                .Where(g => g.StudentId == StudentId)
                .Include(g => g.Subject)
                .Include(g => g.Student)
                    .ThenInclude(s => s.User)
                .GroupBy(g => new { g.SubjectId, g.Subject.Name })
                .Select(g => new SubjectForGradeDto
                {
                    Name = g.Key.Name,
                    SubjectId = g.Key.SubjectId,
                    grade = g.Select(g => new GradeDto
                    {
                        GradeId = g.GradeId,
                        Value = g.Value,
                        Data = g.Data,
                        Student = new StudentForGradeDto
                        {
                            StudentId = g.Student.StudentId,
                            Name = g.Student.User.Name,
                        }
                    }).ToList()
                })
                .ToListAsync();

            if (grade == null)
            {
                return NotFound();
            }

            return grade;
        }

        /// <summary>
        /// Удалить оценку по ее id
        /// </summary>
        /// <param name="id">id оценки</param>
        /// <returns></returns>
        [Authorize(Roles = "Учитель")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(Guid id)
        {
            var grade = await _context.Grade.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }
        
            _context.Grade.Remove(grade);
            await _context.SaveChangesAsync();
        
            return NoContent();
        }
    }
}

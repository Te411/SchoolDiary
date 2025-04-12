using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTO.Grade;
using MyWebApplicationServer.DTO.Student;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Controllers
{
    [Route("api/Grade")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly LibraryContext _context;

        public GradesController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Grades
        /// </summary>
        /// <returns></returns>
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
        /// GET: api/Grades/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// GET: api/Grades/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// PUT: api/Grades/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
       //[HttpPut("{id}")]
       //public async Task<IActionResult> PutGrade(Guid id, Grade grade)
       //{
       //    if (id != grade.GradeId)
       //    {
       //        return BadRequest();
       //    }
       //
       //    _context.Entry(grade).State = EntityState.Modified;
       //
       //    try
       //    {
       //        await _context.SaveChangesAsync();
       //    }
       //    catch (DbUpdateConcurrencyException)
       //    {
       //        if (!GradeExists(id))
       //        {
       //            return NotFound();
       //        }
       //        else
       //        {
       //            throw;
       //        }
       //    }
       //
       //    return NoContent();
       //}

        /// <summary>
        /// POST: api/Grades
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<GradeDto>> PostGrade(CreateGradeDto createGradeDto)
        {
            var subjectExists = await _context.Subject.AnyAsync(s => s.SubjectId == createGradeDto.SubjectId);

            if (!subjectExists)
            {
                return BadRequest("Указанный предмет не существует");
            }

            var studentExists = await _context.Student.AnyAsync(s => s.StudentId == createGradeDto.StudentId);

            if (!studentExists)
            {
                return BadRequest("Указанный студент не существует");
            }

            var grade = new Grade
            {
                SubjectId = createGradeDto.SubjectId,
                StudentId = createGradeDto.StudentId,
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
        /// DELETE: api/Grades/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteGrade(Guid id)
        // {
        //     var grade = await _context.Grade.FindAsync(id);
        //     if (grade == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _context.Grade.Remove(grade);
        //     await _context.SaveChangesAsync();
        //
        //     return NoContent();
        // }

        private bool GradeExists(Guid id)
        {
            return _context.Grade.Any(e => e.GradeId == id);
        }
    }
}

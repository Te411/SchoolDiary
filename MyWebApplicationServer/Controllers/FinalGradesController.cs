using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTOs.FinalGrade;
using MyWebApplicationServer.DTOs.Grade;
using MyWebApplicationServer.DTOs.Student;
using MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Controllers
{
    /// <summary>
    /// Контроллер для таблицы "Итоговая оценка"
    /// </summary>
    [Route("api/FinalGrade")]
    [ApiController]
    public class FinalGradesController : ControllerBase
    {
        private readonly LibraryContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public FinalGradesController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить все итоговые оценки
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinalGradeDto>>> GetFinalGrade()
        {
            return await _context.FinalGrade
                .Include(fg => fg.Subject)
                .Include(fg => fg.Student)
                    .ThenInclude(s => s.User)
                .Select(fg => new FinalGradeDto
                {
                    FinalGradeId = fg.FinalGradeId,
                    StudentId = fg.StudentId,
                    SubjectId = fg.SubjectId,
                    SubjectName = fg.Subject.Name,
                    PeriodType = fg.PeriodType,
                    Quarter = fg.Quarter,
                    GradeValue = fg.GradeValue,
                    Student = new StudentForGradeDto
                    {
                        StudentId = fg.Student.StudentId,
                        Name = fg.Student.User.Name,
                    }
                })
                .ToListAsync();
        }

        /// <summary>
        /// Получить итоговую оценку по ID
        /// </summary>
        /// <param name="finalGradeid"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{finalGradeid}")]
        public async Task<ActionResult<FinalGradeDto>> GetFinalGradeById(Guid finalGradeid)
        {
            var finalGrade = await _context.FinalGrade
                .Where(fg => fg.FinalGradeId == finalGradeid)
                .Include(fg => fg.Subject)
                .Include(fg => fg.Student)
                    .ThenInclude(s => s.User)
                .Select(fg => new FinalGradeDto
                {
                    FinalGradeId = fg.FinalGradeId,
                    StudentId = fg.StudentId,
                    SubjectId = fg.SubjectId,
                    SubjectName = fg.Subject.Name,
                    PeriodType = fg.PeriodType,
                    Quarter = fg.Quarter,
                    GradeValue = fg.GradeValue,
                    Student = new StudentForGradeDto
                    {
                        StudentId = fg.Student.StudentId,
                        Name = fg.Student.User.Name,
                    }
                })
                .FirstOrDefaultAsync();

            if (finalGrade == null)
            {
                return NotFound();
            }

            return finalGrade;
        }

        /// <summary>
        /// Получить итоговые оценки по ID студента
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("FinalGradeByStudentId/{studentId}")]
        public async Task<ActionResult<IEnumerable<FinalGradeDto>>> GetFinalGradeByStudent(Guid studentId)
        {
            var finalGrade = await _context.FinalGrade
                .Where(fg => fg.StudentId == studentId)
                .Include(fg => fg.Subject)
                .Include(fg => fg.Student)
                    .ThenInclude(s => s.User)
                .Select(fg => new FinalGradeDto
                {
                    FinalGradeId = fg.FinalGradeId,
                    StudentId = fg.StudentId,
                    SubjectId = fg.SubjectId,
                    SubjectName = fg.Subject.Name,
                    PeriodType = fg.PeriodType,
                    Quarter = fg.Quarter,
                    GradeValue = fg.GradeValue,
                    Student = new StudentForGradeDto
                    {
                        StudentId = fg.Student.StudentId,
                        Name = fg.Student.User.Name,
                    }
                })
                .ToListAsync();

            if (finalGrade == null)
            {
                return NotFound();
            }

            return finalGrade;
        }

        /// <summary>
        /// Добавить итоговую оценку
        /// </summary>
        /// <param name="createFinalGradeDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Учитель")]
        [HttpPost("CreateFinalGrade")]
        public async Task<IActionResult> AddFinalGrade([FromBody] CreateFinalGradeDto createFinalGradeDto)
        {
            if (createFinalGradeDto.PeriodType == "Четверть" && (createFinalGradeDto.Quarter < 1 || createFinalGradeDto.Quarter > 4))
            {
                return BadRequest("Для четверти должен быть указан номер от 1 до 4");
            }
            var studentExists = await _context.Student.AnyAsync(s => s.StudentId == createFinalGradeDto.StudentId);
            var subjectExists = await _context.Subject.AnyAsync(s => s.SubjectId == createFinalGradeDto.SubjectId);

            if (!studentExists)
            {
                return NotFound("Такого студента не существует!");
            }
            if (!subjectExists)
            {
                return NotFound("Такого предмета не существует");
            }

            var existingGrade = await _context.FinalGrade
                .FirstOrDefaultAsync(fg =>
                    fg.StudentId == createFinalGradeDto.StudentId &&
                    fg.SubjectId == createFinalGradeDto.SubjectId &&
                    fg.PeriodType == createFinalGradeDto.PeriodType &&
                    fg.Quarter == createFinalGradeDto.Quarter);

            if (existingGrade != null)
            {
                return Conflict("Итоговая оценка для указанного периода уже существует");
            }

            var finalGrade = new FinalGrade
            {
                StudentId = createFinalGradeDto.StudentId,
                SubjectId = createFinalGradeDto.SubjectId,
                PeriodType = createFinalGradeDto.PeriodType,
                Quarter = createFinalGradeDto.Quarter,
                GradeValue = createFinalGradeDto.GradeValue
            };

            _context.FinalGrade.Add(finalGrade);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFinalGrade), new { id = finalGrade.FinalGradeId }, finalGrade);
        }
    }
}

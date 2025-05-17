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
using MyWebApplicationServer.Interfaces;
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
        private readonly IFinalGradeRepository _finalGradeRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IStudentRepository _studentRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="finalGradeRepository"></param>
        /// <param name="subjectRepository"></param>
        /// <param name="studentRepository"></param>
        public FinalGradesController(IFinalGradeRepository finalGradeRepository,
            ISubjectRepository subjectRepository, IStudentRepository studentRepository)
        {
            _finalGradeRepository = finalGradeRepository;
            _subjectRepository = subjectRepository;
            _studentRepository = studentRepository;
        }

        /// <summary>
        /// Получить все итоговые оценки
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinalGradeDto>>> GetFinalGrade()
        {
            var finalGrades = await _finalGradeRepository.GetAll();

            if(finalGrades == null)
            {
                return NotFound();
            }

            return Ok(finalGrades);
        }

        /// <summary>
        /// Получить итоговую оценку по ID
        /// </summary>
        /// <param name="finalGradeId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{finalGradeId}")]
        public async Task<ActionResult<FinalGradeDto>> GetFinalGradeById(Guid finalGradeId)
        {
            var finalGrade = await _finalGradeRepository.GetById(finalGradeId);

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
            var finalGrades = await _finalGradeRepository.GetByStudentId(studentId);

            if (finalGrades == null)
            {
                return NotFound();
            }

            return Ok(finalGrades);
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

            var student = await _studentRepository.GetById(createFinalGradeDto.StudentId);

            if (student == null)
            {
                return NotFound("Такого студента не существует!");
            }

            var subject = await _subjectRepository.GetById(createFinalGradeDto.SubjectId);
            if (subject == null)
            {
                return NotFound("Такого предмета не существует");
            }

            var existingGrade = await _finalGradeRepository
                .CheckExistingGrade(
                createFinalGradeDto.StudentId,
                createFinalGradeDto.SubjectId, 
                createFinalGradeDto.PeriodType, 
                createFinalGradeDto.Quarter);

            if (existingGrade)
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

            await _finalGradeRepository.Add(finalGrade);

            return CreatedAtAction(nameof(GetFinalGrade), new { id = finalGrade.FinalGradeId }, finalGrade);
        }
    }
}

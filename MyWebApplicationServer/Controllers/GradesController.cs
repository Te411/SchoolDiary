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
using MyWebApplicationServer.Interfaces;
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
        private readonly IGradeRepository _gradeRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IStudentRepository _studentRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="gradeRepository"></param>
        /// <param name="subjectRepository"></param>
        /// <param name="studentRepository"></param>
        public GradesController(IGradeRepository gradeRepository, 
            ISubjectRepository subjectRepository, IStudentRepository studentRepository)
        {
            _gradeRepository = gradeRepository;
            _subjectRepository = subjectRepository;
            _studentRepository = studentRepository;
        }

        /// <summary>
        /// Получить все существующие оценки
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GradeDto>>> GetGrade()
        {
            var allGrades = await _gradeRepository.GetAll();

            if(allGrades == null)
            {
                return NotFound();
            }

            return Ok(allGrades);
        }

        /// <summary>
        /// Получить оценку по id
        /// </summary>
        /// <param name="id">id Оценки</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("id/{id}")]
        public async Task<ActionResult<IEnumerable<GradeDto>>> GetGradeById(Guid id)
        {
            var grade = await _gradeRepository.GetById(id);

            if (grade == null)
            {
                return NotFound();
            }

            return Ok(grade);
        }

        /// <summary>
        /// Получить все оценки по названию предмета
        /// </summary>
        /// <param name="subjectName">Название предмета</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("subjectName/{subjectName}")]
        public async Task<ActionResult<IEnumerable<GradeDto>>> GetGradeBySubjectName(string subjectName)
        {
            var grades = await _gradeRepository.GetBySubjectName(subjectName);

            if (grades == null)
            {
                return NotFound();
            }

            return Ok(grades);
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
            var subject = await _subjectRepository.GetByNameAsync(createGradeDto.SubjectName);

            if (subject == null)
            {
                return BadRequest("Указанный предмет не существует");
            }

            var student = await _studentRepository.GetByStudentName(createGradeDto.StudentName);

            if (student == null)
            {
                return BadRequest("Указанный студент не существует");
            }

            var grade = new Grade
            {
                SubjectId = subject.SubjectId,
                StudentId = student.StudentId,
                Value = createGradeDto.Value,
                Data = createGradeDto.Data
            };

            var createdGrade = await _gradeRepository.Add(grade);

            var gradeDto = new GradeDto
            {
                GradeId = createdGrade.GradeId,
                SubjectId = createdGrade.SubjectId,
                SubjectName = subject.Name,
                Value = createdGrade.Value,
                Data = createdGrade.Data,
                Student = new StudentForGradeDto
                {
                    StudentId = student.StudentId,
                    Name = student.User.Name,
                }
            };

            return CreatedAtAction("GetGrade", new { id = gradeDto.GradeId }, gradeDto);
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
            var grade = await _gradeRepository.GetByClassIdAndSubjectId(gradeAllStudent.ClassId, gradeAllStudent.SubjectId);

            if (grade == null)
            {
                return NotFound();
            }

            return Ok(grade);
        }

        /// <summary>
        /// Получить все оценки по ID студента
        /// </summary>
        /// <param name="userId">ID студента</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("userId/{userId}")]
        public async Task<ActionResult<IEnumerable<SubjectForGradeDto>>> GetGradeByStudentId(Guid userId)
        {
            var grade = await _gradeRepository.GetByUserId(userId);

            if (grade == null)
            {
                return NotFound();
            }

            return Ok(grade);
        }

        /// <summary>
        /// Удалить оценку по ее ID
        /// </summary>
        /// <param name="id">ID оценки</param>
        /// <returns></returns>
        [Authorize(Roles = "Учитель")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(Guid id)
        {
            await _gradeRepository.Delete(id);
            return NoContent();
        }
    }
}

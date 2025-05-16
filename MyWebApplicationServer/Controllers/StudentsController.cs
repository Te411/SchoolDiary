using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTOs.Student;
using MyWebApplicationServer.DTOs.User;
using MyWebApplicationServer.Interfaces;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Controllers
{
    /// <summary>
    /// Контроллер для таблицы "Студент"
    /// </summary>
    [Route("api/Student")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="studentRepository"></param>
        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        /// <summary>
        /// Получить всех студентов
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentForRoleDto>>> GetStudent()
        {
            var allStudents = await _studentRepository.GetAllStudent();

            if(allStudents == null)
            {
                return NotFound();
            }

            return Ok(allStudents);
        }

        /// <summary>
        /// Получить студента по его id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(Guid id)
        {
            var student = await _studentRepository.GetById(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        /// <summary>
        /// Получить студента по User id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("userId/{userId}")]
        public async Task<ActionResult<IEnumerable<StudentForGradeDto>>> GetStudentByUserId(Guid userId)
        {
            var student = await _studentRepository.GetByUserId(userId);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        /// <summary>
        /// Получить студентов по названию класса
        /// </summary>
        /// <param name="className">Название класса</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("ClassName/{className}")]
        public async Task<ActionResult<IEnumerable<StudentForGradeDto>>> GetStudentByClassName(string className)
        {
            var student = await _studentRepository.GetByClassName(className);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        /// <summary>
        /// Добавить нового студента
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [Authorize(Roles = "Завуч")]
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            var createdStudent = await _studentRepository.Add(student);

            return CreatedAtAction("GetStudent", new { id = createdStudent.StudentId }, createdStudent);
        }

        /// <summary>
        /// Удалить студента
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Завуч")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            await _studentRepository.Delete(id);
            return NoContent();
        }

        /// <summary>
        /// Получить общую информацию о студенте
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GeneralInfo/{id}")]
        public async Task<ActionResult<IEnumerable<StudentGeneralInfoDto>>> GetGeneralInfoStudent(Guid id)
        {
            var generalInfoStudent = await _studentRepository.GetGeneralInfoById(id);

            if (generalInfoStudent == null)
            {
                return NotFound();
            }

            return Ok(generalInfoStudent);
        }
    }
}

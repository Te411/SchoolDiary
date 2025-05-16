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
using MyWebApplicationServer.DTOs.Teacher;
using MyWebApplicationServer.Interfaces;
using NuGet.DependencyResolver;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Controllers
{
    /// <summary>
    /// Контроллер для таблицы "Учитель"
    /// </summary>
    [Route("api/Teacher")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherRepository _teacherRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="teacherRepository"></param>
        public TeachersController(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        /// <summary>
        /// Получить общую информацию о учителе
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GeneralInfo/{userId}")]
        public async Task<ActionResult<TeacherGeneralInfoDto>> GetGeneralInfoTeacher(Guid userId)
        {
            var teacher = await _teacherRepository.GetTeacherInfoByUserIdAsync(userId);

            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(teacher);
        }
    }
}

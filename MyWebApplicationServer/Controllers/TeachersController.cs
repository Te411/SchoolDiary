using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTOs.Student;
using MyWebApplicationServer.DTOs.Teacher;
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
        private readonly LibraryContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public TeachersController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить общую информацию о учителе
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GeneralInfo/{userId}")]
        public async Task<ActionResult<IEnumerable<TeacherGeneralInfoDto>>> GetGeneralInfoTeacher(Guid userId)
        {
            var teacher = await _context.Teacher
                .Where(t => t.UserId == userId)
                .Include(t => t.User)
                .Select(t => new TeacherGeneralInfoDto
                {
                    Name = t.User.Name,
                    Email = t.User.Email,
                })
                .ToListAsync();

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTO.Student;
using MyWebApplicationServer.DTO.Teacher;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly LibraryContext _context;

        public TeachersController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить общую информацию о учителе
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GeneralInfo/{id}")]
        public async Task<ActionResult<IEnumerable<TeacherGeneralInfo>>> GetGeneralInfoTeacher(Guid id)
        {
            var teacher = await _context.Teacher
                .Where(t => t.TeacherId == id)
                .Include(t => t.User)
                .Select(t => new TeacherGeneralInfo
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

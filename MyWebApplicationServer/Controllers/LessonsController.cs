using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTO.Lesson;
using MyWebApplicationServer.DTO.Subject;
using MyWebApplicationServer.DTO.Teacher;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Controllers
{
    [Route("api/Lesson")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LessonsController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Lessons
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetLesson()
        {
            return await _context.Lesson
                .Include(l=>l.Teacher)
                    .ThenInclude(t=>t.User)
                .Include(l=>l.Subject)
                .Select(l => new LessonDto
                {
                    LessonId = l.LessonId,
                    SubjectId = l.SubjectId,
                    TeacherId = l.TeacherId,
                    StartTime = l.StartTime,
                    EndTime = l.EndTime,
                    Homework = l.Homework,
                    Room = l.Room,
                    Subject = new SubjectForLessonDto
                    {
                        Name = l.Subject.Name
                    },
                    Teacher = new TeacherForLessonDto
                    {
                        Email = l.Teacher.User.Email,
                        Name = l.Teacher.User.Name
                    }
                })
                .ToListAsync();
        }

        /// <summary>
        /// GET: api/Lessons/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{subjectName}")]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetLesson(string subjectName)
        {
            var lesson = await _context.Lesson
                .Where(l => l.Subject.Name == subjectName)
                .Include(l => l.Subject)
                .Include(l=>l.Teacher)
                    .ThenInclude(t => t.User)
                .Select(l => new LessonDto
                {
                    LessonId = l.LessonId,
                    SubjectId = l.SubjectId,
                    TeacherId = l.TeacherId,
                    StartTime = l.StartTime,
                    EndTime = l.EndTime,
                    Homework = l.Homework,
                    Room = l.Room,
                    Subject = new SubjectForLessonDto
                    {
                        Name = l.Subject.Name
                    },
                    Teacher = new TeacherForLessonDto
                    {
                        Email = l.Teacher.User.Email,
                        Name = l.Teacher.User.Name
                    }
                })
                .ToListAsync();
                

            if (lesson == null)
            {
                return NotFound();
            }

            return lesson;
        }
    }
}

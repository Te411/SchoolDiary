using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTO.Lesson;
using MyWebApplicationServer.DTO.Schedule;
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
        /// Получить все уроки
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
        /// получить уроки по названию предмета
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

        /// <summary>
        /// Обновить домашнее задание для урока
        /// </summary>
        /// <param name="addHomeworkDto"></param>
        /// <returns></returns>
        [HttpPatch("UpdateHomeworkByLessonId")]
        public async Task<IActionResult> UpdateHomework([FromBody] AddHomeworkLessonDto addHomeworkDto)
        {
            try
            {
                var lessonEntity = await _context.Lesson
                    .FirstOrDefaultAsync(c => c.LessonId == addHomeworkDto.LessonId);

                if (lessonEntity == null)
                {
                    return NotFound($"Урок '{addHomeworkDto.LessonId}' не найден");
                }

                lessonEntity.Homework = addHomeworkDto.Homework;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch(DbUpdateConcurrencyException ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Ошибка при обновлении данных",
                    Details = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Непредвиденная ошибка",
                    Details = ex.Message
                });
            }
        }
    }
}

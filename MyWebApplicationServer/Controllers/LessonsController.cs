using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTOs.Lesson;
using MyWebApplicationServer.DTOs.Schedule;
using MyWebApplicationServer.DTOs.Subject;
using MyWebApplicationServer.DTOs.Teacher;
using MyWebApplicationServer.DTOs.Room;
using Project.MyWebApplicationServer.Models;
using Microsoft.AspNetCore.Authorization;

namespace MyWebApplicationServer.Controllers
{
    /// <summary>
    /// Контроллер для таблицы "Урок"
    /// </summary>
    [Route("api/Lesson")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly LibraryContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public LessonsController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить все уроки
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetLesson()
        {
            return await _context.Lesson
                .Include(l=>l.Teacher)
                    .ThenInclude(t=>t.User)
                .Include(l=>l.Subject)
                .Include(l => l.Room)
                .Select(l => new LessonDto
                {
                    LessonId = l.LessonId,
                    SubjectId = l.SubjectId,
                    TeacherId = l.TeacherId,
                    StartTime = l.StartTime,
                    EndTime = l.EndTime,
                    Homework = l.Homework,
                    Room = l.Room != null ? new RoomDto
                    {
                        Name = l.Room.Name,
                    } : null,
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
        /// Получить все кабинеты в которых проходят уроки
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Room")]
        public async Task<ActionResult<IEnumerable<LessonRoomDto>>> GetRoom()
        {
            return await _context.Lesson
                .Select(l => new LessonRoomDto
                {
                    RoomName = l.Room.Name,
                })
                .Distinct()
                .ToListAsync();
        }

        /// <summary>
        /// получить уроки по названию предмета
        /// </summary>
        /// <param name="subjectName"></param>
        /// <returns></returns>
        [Authorize]
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
                    Room = l.Room != null ? new RoomDto
                    {
                        Name = l.Room.Name,
                    } : null,
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
        [Authorize(Roles = "Учитель")]
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

        /// <summary>
        /// Удаление урока
        /// </summary>
        /// <param name="lessonId"></param>
        /// <returns></returns>
        //[HttpDelete("ByClassLesson/{lessonId}")]
        //public async Task<IActionResult> DeleteLesson(Guid lessonId)
        //{
        //    var LessonEntity = await _context.Lesson
        //        .FirstOrDefaultAsync(l => l.LessonId == lessonId);

        //    if (LessonEntity == null)
        //    {
        //        return NotFound($"Урок '{lessonId}' не найден.");
        //    }

        //    _context.Lesson.Remove(LessonEntity);
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}
    }
}

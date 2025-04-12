using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using Project.MyWebApplicationServer.Models;
using MyWebApplicationServer.DTO.Schedule;
using MyWebApplicationServer.DTO.Lesson;
using Humanizer;

namespace MyWebApplicationServer.Controllers
{
    [Route("api/Schedules")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly LibraryContext _context;

        public SchedulesController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Schedules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedule()
        {
            var schedule = await _context.Schedule
                .Include(s => s.Class)
                .Include(s => s.WeekDay)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Subject)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Teacher)
                        .ThenInclude(t => t.User)
                .OrderBy(s => s.WeekDayId)
                .ThenBy(s => s.LessonOrder)
                .ToListAsync();

            if (schedule == null || !schedule.Any())
            {
                return NotFound();
            }

            return schedule;
        }

        /// <summary>
        /// GET: api/ByClassId/Schedules/5
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpGet("ByClassId/{classId}")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedule(Guid classId)
        {
            var schedule = await _context.Schedule
                .Where(s => s.ClassId == classId)
                .Include(s => s.Class)
                .Include(s => s.WeekDay)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Subject)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Teacher)
                        .ThenInclude(t => t.User)
                .OrderBy(s => s.WeekDayId)
                .ThenBy(s => s.LessonOrder)
                .ToListAsync();

            if (schedule == null || !schedule.Any())
            {
                return NotFound();
            }

            return schedule;
        }

        /// <summary>
        /// GET: api/ByClassName/Schedules/5
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        [HttpGet("ByClassName/{className}")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedule(string className)
        {
            var schedule = await _context.Schedule
                .Where(s => s.Class.Name == className)
                .Include(s => s.Class)
                .Include(s => s.WeekDay)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Subject)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Teacher)
                        .ThenInclude(t => t.User)
                .OrderBy(s => s.WeekDayId)
                .ThenBy(s => s.LessonOrder)
                .ToListAsync();

            if (schedule == null || !schedule.Any())
            {
                return NotFound();
            }

            return schedule;
        }

        /// <summary>
        /// GET: api/Schedules/ByClassName/Correct/Schedules/5
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        [HttpGet("ByClassName/Correct/{className}")]
        public async Task<ActionResult<List<ScheduleDto>>> GetCorrectSchedule(string className)
        {
            var schedule = await _context.Schedule
                .Where(s => s.Class.Name == className)
                .Include(s => s.WeekDay)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Subject)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Teacher)
                        .ThenInclude(t => t.User)
                .OrderBy(s => s.WeekDayId)
                .ThenBy(s => s.LessonOrder)
                .ToListAsync();

            if (schedule == null || !schedule.Any())
            {
                return NotFound();
            }

            var result = schedule
                .GroupBy(s => new { s.WeekDayId, s.WeekDay.Name })
                .Select(g => new ScheduleDto
                {
                    WeekDayName = g.Key.Name,
                    Lessons = g.Select(s => new LessonForScheduleDto
                    {
                        LessonOrder = s.LessonOrder,
                        SubjectName = s.Lesson.Subject.Name,
                        TeacherName = s.Lesson.Teacher.User.Name,
                        StartTime = s.Lesson.StartTime,
                        EndTime = s.Lesson.EndTime,
                        Homework = s.Lesson.Homework,
                        Room = s.Lesson.Room
                    }).ToList()
                })
                .ToList();

            return result;
        }

        /// <summary>
        /// PUT: api/Schedules/UpdateHomeworkByClassName
        /// </summary>
        /// <param name="addHomeworkDto"></param>
        /// <returns></returns>
        [HttpPatch("UpdateHomeworkByClassName")]
        public async Task<IActionResult> UpdateHomework([FromBody] AddHomeworkScheduleDto addHomeworkDto)
        {
            try
            {
                var classEntity = await _context.Class
                    .FirstOrDefaultAsync(c => c.Name == addHomeworkDto.ClassName);

                if (classEntity == null)
                {
                    return NotFound($"Класс с именем '{addHomeworkDto.ClassName}' не найден");
                }

                var schedule = await _context.Schedule
                    .FirstOrDefaultAsync(s =>
                    s.ClassId == classEntity.ClassId &&
                    s.WeekDayId == addHomeworkDto.WeekDayId &&
                    s.LessonOrder == addHomeworkDto.LessonOrder);

                if (schedule == null)
                {
                    return NotFound("Запись в расписании не найдена");
                }

                var lesson = await _context.Lesson.FindAsync(schedule.LessonId);
                if (lesson == null)
                {
                    return NotFound("Урок не найден");
                }

                lesson.Homework = addHomeworkDto.Homework;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                { 
                    Message = "Ошибка при обновлении данных",
                    Details = ex.Message 
                });
            }
            catch(Exception ex)
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

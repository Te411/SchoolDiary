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
        /// Получить все расписания
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleWeekByClassDto>>> GetSchedule()
        {
            var schedule = await _context.Schedule
                .Include(s => s.Class)
                .Include(s => s.WeekDay)
                .Include(s => s.Week)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Subject)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Teacher)
                        .ThenInclude(t => t.User)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Room)
                .OrderBy(s => s.WeekDayId)
                .ThenBy(s => s.LessonOrder)
                .ToListAsync();

            if (schedule == null || !schedule.Any())
            {
                return NotFound();
            }

            var result = schedule
                .GroupBy(s => new { s.WeekId, s.Week.StartDate, s.Week.EndDate, s.Class.ClassId })
                .Select(weekGroup => new ScheduleWeekByClassDto
                {
                    WeekId = weekGroup.Key.WeekId,
                    StartDate = weekGroup.Key.StartDate,
                    EndDate = weekGroup.Key.EndDate,
                    Schedule = weekGroup
                        .GroupBy(s => new { s.WeekDayId, s.WeekDay.Name })
                        .Select(dayGroup => new ScheduleWeekDayByClassDto
                        {
                            WeekDayName = dayGroup.Key.Name,
                            Lessons = dayGroup
                                .OrderBy(l => l.LessonOrder)
                                .Select(s => new LessonForScheduleByClassDto
                                {
                                    LessonOrder = s.LessonOrder,
                                    SubjectName = s.Lesson.Subject.Name,
                                    TeacherName = s.Lesson.Teacher.User.Name,
                                    StartTime = s.Lesson.StartTime,
                                    EndTime = s.Lesson.EndTime,
                                    Homework = s.Lesson.Homework ?? null,
                                    RoomName = s.Lesson.Room != null ? s.Lesson.Room.Name.Trim() : null
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            return result;
        }

        /// <summary>
        /// Получить расписание по id класса и id недели
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpGet("ByClassId/{classId}/{weekId}")]
        public async Task<ActionResult<List<ScheduleWeekByClassDto>>> GetScheduleByClass(Guid classId, int weekId)
        {
            var schedule = await _context.Schedule
                .Where(s => s.ClassId == classId &&
                s.WeekId == weekId)
                .Include(s => s.Class)
                .Include(s => s.WeekDay)
                .Include(s => s.Week)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Subject)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Teacher)
                        .ThenInclude(t => t.User)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Room)
                .OrderBy(s => s.WeekDayId)
                .ThenBy(s => s.LessonOrder)
                .ToListAsync();

            if (schedule == null || !schedule.Any())
            {
                return NotFound();
            }

            var result = schedule
                .GroupBy(s => new { s.WeekId, s.Week.StartDate, s.Week.EndDate })
                .Select(weekGroup => new ScheduleWeekByClassDto
                {
                    WeekId = weekGroup.Key.WeekId,
                    StartDate = weekGroup.Key.StartDate,
                    EndDate = weekGroup.Key.EndDate,
                    Schedule = weekGroup
                        .GroupBy(s => new { s.WeekDayId, s.WeekDay.Name })
                        .Select(dayGroup => new ScheduleWeekDayByClassDto
                        {
                            WeekDayName = dayGroup.Key.Name,
                            Lessons = dayGroup
                                .OrderBy(l => l.LessonOrder)
                                .Select(s => new LessonForScheduleByClassDto
                                {
                                    LessonId = s.LessonId,
                                    LessonOrder = s.LessonOrder,
                                    SubjectName = s.Lesson.Subject.Name,
                                    TeacherName = s.Lesson.Teacher.User.Name,
                                    StartTime = s.Lesson.StartTime,
                                    EndTime = s.Lesson.EndTime,
                                    Homework = s.Lesson.Homework ?? null,
                                    RoomName = s.Lesson.Room != null ? s.Lesson.Room.Name.Trim() : null
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            return result;
        }

        /// <summary>
        /// Получить расписание по id пользователя и id недели
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpGet("ByTeacher/{userId}/{weekId}")]
        public async Task<ActionResult<List<ScheduleWeekByTeacherDto>>> GetScheduleByTeacher(Guid userId, int weekId)
        {
            var teacher = await _context.Teacher.FirstOrDefaultAsync(t => t.UserId == userId);

            if (teacher == null) 
            {
                return BadRequest($"Учителя с id не найдено");
            }

            var schedule = await _context.Schedule
                    .Where(s => s.Lesson.TeacherId == teacher.TeacherId &&
                                s.WeekId == weekId)
                    .Include(s => s.Class)
                    .Include(s => s.WeekDay)
                    .Include(s => s.Week)
                    .Include(s => s.Lesson)
                        .ThenInclude(l => l.Subject)
                    .Include(s => s.Lesson)
                        .ThenInclude(l => l.Teacher)
                            .ThenInclude(t => t.User)
                     .Include(s => s.Lesson)
                        .ThenInclude(l => l.Room)
                    .OrderBy(s => s.WeekDayId)
                    .ThenBy(s => s.LessonOrder)
                    .ToListAsync();

            if (schedule == null || !schedule.Any())
            {
                return NotFound();
            }

            var result = schedule
                .GroupBy(s => new { s.WeekId, s.Week.StartDate, s.Week.EndDate })
                .Select(weekGroup => new ScheduleWeekByTeacherDto
                {
                    WeekId = weekGroup.Key.WeekId,
                    StartDate = weekGroup.Key.StartDate,
                    EndDate = weekGroup.Key.EndDate,
                    Schedule = weekGroup
                        .GroupBy(s => new { s.WeekDayId, s.WeekDay.Name })
                        .Select(dayGroup => new ScheduleWeekDayByTeacherDto
                        {
                            WeekDayName = dayGroup.Key.Name,
                            Lessons = dayGroup
                                .OrderBy(l => l.LessonOrder)
                                .Select(s => new LessonForScheduleByTeacherDto
                                {
                                    LessonId = s.LessonId,
                                    ClassName = s.Class.Name,
                                    LessonOrder = s.LessonOrder,
                                    SubjectName = s.Lesson.Subject.Name,
                                    StartTime = s.Lesson.StartTime,
                                    EndTime = s.Lesson.EndTime,
                                    Homework = s.Lesson.Homework ?? null,
                                    RoomName = s.Lesson.Room != null ? s.Lesson.Room.Name.Trim() : null
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            return result;
        }




        /// <summary>
        /// Получить расписание по имени класса и id недели
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        [HttpGet("ByClassName/Correct/{className}/{weekId}")]
        public async Task<ActionResult<List<ScheduleWeekByClassDto>>> GetCorrectSchedule(string className, int weekId)
        {
            var schedule = await _context.Schedule
                .Where(s => s.Class.Name == className &&
                s.WeekId == weekId)
                .Include(s => s.WeekDay)
                .Include (s => s.Week)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Subject)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Teacher)
                        .ThenInclude(t => t.User)
                .Include(s => s.Lesson)
                    .ThenInclude(l => l.Room)
                .OrderBy(s => s.WeekDayId)
                .ThenBy(s => s.LessonOrder)
                .ToListAsync();

            if (schedule == null || !schedule.Any())
            {
                return NotFound();
            }

            var result = schedule
                .GroupBy(s => new { s.WeekId, s.Week.StartDate, s.Week.EndDate })
                .Select(weekGroup => new ScheduleWeekByClassDto
                {
                    WeekId = weekGroup.Key.WeekId,
                    StartDate = weekGroup.Key.StartDate,
                    EndDate = weekGroup.Key.EndDate,
                    Schedule = weekGroup
                        .GroupBy(s => new { s.WeekDayId, s.WeekDay.Name })
                        .Select(dayGroup => new ScheduleWeekDayByClassDto
                        {
                            WeekDayName = dayGroup.Key.Name,
                            Lessons = dayGroup
                                .OrderBy(l => l.LessonOrder)
                                .Select(s => new LessonForScheduleByClassDto
                                {
                                    LessonId = s.LessonId,
                                    LessonOrder = s.LessonOrder,
                                    SubjectName = s.Lesson.Subject.Name,
                                    TeacherName = s.Lesson.Teacher.User.Name,
                                    StartTime = s.Lesson.StartTime,
                                    EndTime = s.Lesson.EndTime,
                                    Homework = s.Lesson.Homework ?? null,
                                    RoomName = s.Lesson.Room != null ? s.Lesson.Room.Name.Trim() : null
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            return result;
        }

        /// <summary>
        /// Обновить домашнее задание урока по данным расписания
        /// </summary>
        /// <param name="addHomeworkDto"></param>
        /// <returns></returns>
        [HttpPatch("UpdateHomeworkByClassName")]
        public async Task<IActionResult> UpdateHomework([FromBody] AddHomeworkScheduleDto addHomeworkDto)
        {
            try
            {
                var weekEntity = await _context.Week
                    .FirstOrDefaultAsync(w => w.WeekId == addHomeworkDto.WeekId);

                if (weekEntity == null)
                {
                    return NotFound($"Неделя не найдена");
                }

                var classEntity = await _context.Class
                    .FirstOrDefaultAsync(c => c.Name == addHomeworkDto.ClassName);

                if (classEntity == null)
                {
                    return NotFound($"Класс не найден");
                }

                var schedule = await _context.Schedule
                    .FirstOrDefaultAsync(s =>
                    s.ClassId == classEntity.ClassId &&
                    s.WeekDayId == addHomeworkDto.WeekDayId &&
                    s.LessonOrder == addHomeworkDto.LessonOrder &&
                    s.WeekId == addHomeworkDto.WeekId);

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

        /// <summary>
        /// Проверка на пересечение уроков
        /// </summary>
        /// <param name="newStartTime"></param>
        /// <param name="newEndTime"></param>
        /// <param name="existingStartTime"></param>
        /// <param name="existingEndTime"></param>
        /// <returns></returns>
        private bool IsTimeConflicting(TimeSpan newStartTime, TimeSpan newEndTime, TimeSpan existingStartTime, TimeSpan existingEndTime)
        {
            return newStartTime < existingEndTime && newEndTime > existingStartTime;
        }

        /// <summary>
        /// Добавление расписания из существующих уроков
        /// </summary>
        /// <param name="addScheduleDto"></param>
        /// <returns></returns>
        [HttpPost("WithOldLessons")]
        public async Task<IActionResult> PostScheduleWithOldLesson([FromBody] AddScheduleDto addScheduleDto)
        {
            try
            {
                var weekEntity = await _context.Week
                    .FirstOrDefaultAsync(w => w.WeekId == addScheduleDto.WeekId);

                if (weekEntity == null)
                {
                    return NotFound($"Недели не найдено");
                }

                var classEntity = await _context.Class
                    .FirstOrDefaultAsync(c => c.Name == addScheduleDto.ClassName);
       
                if (classEntity == null)
                {
                    return NotFound($"Класса не существует!");
                }
       
                var weekDay = await _context.WeekDay
                    .FirstOrDefaultAsync(w => w.WeekDayId == addScheduleDto.WeekDayId);
       
                if (weekDay == null)
                {
                    return NotFound("Такого дня недели в расписании нет");
                }
       
       
                var lessonsFromPost = addScheduleDto.Lessons.Select(l => l.LessonId).ToList();
                var lessons = await _context.Lesson
                    .Where(l => lessonsFromPost.Contains(l.LessonId))
                    .ToListAsync();
       
                if (lessons.Count != addScheduleDto.Lessons.Count)
                {
                    var missingIds = lessonsFromPost.Except(lessons.Select(l => l.LessonId)).ToList();
                    return BadRequest(new
                    {
                        Message = $"Уроки с такими ID не найдены."
                    });
                }
       
                for (int i = 0; i < lessons.Count; i++)
                {
                    for (int j = i + 1; j < lessons.Count; j++)
                    {
                        if (IsTimeConflicting(lessons[i].StartTime, lessons[i].EndTime,
                            lessons[j].StartTime, lessons[j].EndTime))
                        {
                            return BadRequest($"Уроки '{lessons[i].LessonId}' и '{lessons[j].LessonId}' пересекаются по времени.");
                        }
                    }
                }
       
                var newLessonsOrdered = addScheduleDto.Lessons
                    .Select(dto => new
                    {
                        dto.LessonOrder,
                        Lesson = lessons.First(l => l.LessonId == dto.LessonId)
                    })
                    .OrderBy(x => x.LessonOrder)
                    .ToList();
       
                for (int i = 0; i < newLessonsOrdered.Count - 1; i++)
                {
                    var current = newLessonsOrdered[i];
                    var next = newLessonsOrdered[i + 1];
                    if (current.Lesson.EndTime > next.Lesson.StartTime)
                    {
                        return BadRequest($"Порядок уроков {current.LessonOrder} и {next.LessonOrder} не соответствует времени: урок {current.LessonOrder} заканчивается позже начала урока {next.LessonOrder}.");
                    }
                }
       
                var existingSchedule = await _context.Schedule
                    .Where(s => s.ClassId == classEntity.ClassId && 
                    s.WeekDayId == addScheduleDto.WeekDayId &&
                    s.WeekId == addScheduleDto.WeekId)
                    .Include(s => s.Lesson)
                    .ToListAsync();
       
                foreach (var lesson in lessons)
                {
                    foreach (var existing in existingSchedule)
                    {
                        if (IsTimeConflicting(lesson.StartTime, lesson.EndTime,
                            existing.Lesson.StartTime, existing.Lesson.EndTime))
                        {
                            return BadRequest($"Урок '{lesson.LessonId}' пересекается с существующим уроком '{existing.Lesson.LessonId}'.");
                        }
                    }
                }

                foreach (var lesson in lessons)
                {
                    var teacherConflicts = await _context.Schedule
                        .Include(s => s.Lesson)
                        .Include(s => s.Class)
                        .Where(s => s.Lesson.TeacherId == lesson.TeacherId &&
                                    s.WeekId == addScheduleDto.WeekId &&
                                    s.WeekDayId == addScheduleDto.WeekDayId &&
                                    (lesson.StartTime < s.Lesson.EndTime &&
                                     lesson.EndTime > s.Lesson.StartTime))
                        .ToListAsync();

                    if (teacherConflicts.Any())
                    {
                        var conflict = teacherConflicts.First();
                        return BadRequest($"Учитель уже ведёт урок в это время в другом классе .");
                    }
                }

                var lessonOrders = addScheduleDto.Lessons.Select(l => l.LessonOrder).ToList();
       
                if (lessonOrders.Distinct().Count() != lessonOrders.Count)
                {
                    return BadRequest("Повторяющиеся порядковые номера уроков в запросе.");
                }
       
                var existingOrders = existingSchedule.Select(s => s.LessonOrder).ToHashSet();
                foreach ( var item in addScheduleDto.Lessons)
                {
                    if (existingOrders.Contains(item.LessonOrder))
                    {
                        return BadRequest($"Порядок урока {item.LessonOrder} уже занят.");
                    }
                }
       
                var existingLessonsInSchedule = existingSchedule
                    .Select(s => new
                    {
                        s.LessonOrder,
                        s.Lesson.StartTime,
                        s.Lesson.EndTime
                    })
                    .ToList();
       
                var newLessonsInSchedule = addScheduleDto.Lessons
                    .Select(dto => new
                    {
                        dto.LessonOrder,
                        StartTime = lessons.First(l => l.LessonId == dto.LessonId).StartTime,
                        EndTime = lessons.First(l => l.LessonId == dto.LessonId).EndTime
                    })
                    .ToList();
       
                var allLessons = existingLessonsInSchedule
                    .Concat(newLessonsInSchedule)
                    .OrderBy(x => x.LessonOrder)
                    .ToList();
       
                for (int i = 0; i < allLessons.Count - 1; i++)
                {
                    if (allLessons[i].EndTime > allLessons[i + 1].StartTime)
                    {
                        return BadRequest($"Урок с порядком {allLessons[i].LessonOrder} заканчивается позже начала урока {allLessons[i + 1].LessonOrder}.");
                    }
                }
       
                var newSchedules = addScheduleDto.Lessons.Select(item => new Schedule
                {
                    ScheduleId = Guid.NewGuid(),
                    ClassId = classEntity.ClassId,
                    WeekDayId = addScheduleDto.WeekDayId,
                    LessonId = item.LessonId,
                    WeekId = addScheduleDto.WeekId,
                    LessonOrder = item.LessonOrder
                }).ToList();
       
                await _context.Schedule.AddRangeAsync(newSchedules);
                await _context.SaveChangesAsync();
       
                return CreatedAtAction(nameof(GetSchedule), new { classId = classEntity.ClassId }, newSchedules);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Ошибка при добавлении расписания.",
                    Details = dbEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Непредвиденная ошибка!",
                    Details = ex.Message
                });
            }
        }
 
        /// <summary>
        /// Добавление расписания с созданием новых уроков
        /// </summary>
        /// <param name="addScheduleDto"></param>
        /// <returns></returns>
        [HttpPost("WithNewLessons")]
        public async Task<IActionResult> PostScheduleWithNewLesson([FromBody] AddScheduleNewLessonDto addScheduleDto)
        {
        
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var classEntity = await _context.Class
                    .FirstOrDefaultAsync(c => c.Name == addScheduleDto.ClassName);
        
                if (classEntity == null)
                {
                    return NotFound($"Класса не существует!");
                }
        
                var lessonsByDay = addScheduleDto.Lessons
                    .GroupBy(l => l.WeekDayId)
                    .ToDictionary(g => g.Key, g => g.ToList());
        
                var teacherTimes = new Dictionary<Guid, List<(TimeSpan Start, TimeSpan End)>>();
        
                foreach (var (weekDayId, dayLessons) in lessonsByDay)
                {
                     // var week = await _context.Week
                     //    .FirstOrDefaultAsync(w => w.WeekId == addScheduleDto.WeekId);
                     // 
                     // if (week == null)
                     // {
                     //     return NotFound($"Неделя с ID {addScheduleDto.WeekId} не найдена.");
                     // }
        
                     var weekDay = await _context.WeekDay
                        .FirstOrDefaultAsync(w => w.WeekDayId == weekDayId);
        
                    if (weekDay == null)
                    {
                        return NotFound("Такого дня недели в расписании нет");
                    }
        
                    var lessonOrders = dayLessons.Select(l => l.LessonOrder).ToList();
        
                    if (lessonOrders.Distinct().Count() != lessonOrders.Count)
                    {
                        return BadRequest("Повторяющиеся порядковые номера уроков в запросе.");
                    }
        
                    foreach (var item in dayLessons)
                    {
                        if (item.StartTime >= item.EndTime)
                            return BadRequest($"Некорректное время для урока {item.LessonOrder} (день {weekDayId})");
                    }
        
                    for (int i = 0; i < dayLessons.Count; i++)
                    {
                        for (int j = i + 1; j < dayLessons.Count; j++)
                        {
                            if (IsTimeConflicting(
                                dayLessons[i].StartTime, dayLessons[i].EndTime,
                                dayLessons[j].StartTime, dayLessons[j].EndTime))
                            {
                                return BadRequest($"Конфликт времени между уроками {dayLessons[i].LessonOrder} и {dayLessons[j].LessonOrder} (день {weekDayId})");
                            }
                        }
                    }
        
                    var ordered = dayLessons.OrderBy(l => l.LessonOrder).ToList();
                    for (int i = 0; i < ordered.Count - 1; i++)
                    {
                        if (ordered[i].EndTime > ordered[i + 1].StartTime)
                            return BadRequest($"Некорректный порядок уроков {ordered[i].LessonOrder} и {ordered[i + 1].LessonOrder} (день {weekDayId})");
                    }
        
                    var existingSchedules = await _context.Schedule
                        .Include(s => s.Lesson)
                        .Where(s => s.ClassId == classEntity.ClassId && s.WeekDayId == weekDayId)
                        .ToListAsync();
        
                    var existingOrders = existingSchedules.Select(s => s.LessonOrder).ToHashSet();
                    var conflictOrders = dayLessons.Where(l => existingOrders.Contains(l.LessonOrder)).ToList();
                    if (conflictOrders.Any())
                        return BadRequest($"Занятые номера уроков: {string.Join(", ", conflictOrders.Select(o => o.LessonOrder))} (день {weekDayId})");
        
                    var existingTimeSlots = existingSchedules
                        .Select(s => new { s.Lesson.StartTime, s.Lesson.EndTime })
                        .ToList();
        
                    foreach (var newLesson in dayLessons)
                    {
                        if (existingTimeSlots.Any(ts =>
                            IsTimeConflicting(newLesson.StartTime, newLesson.EndTime, ts.StartTime, ts.EndTime)))
                        {
                            return BadRequest("Конфликт с существующими уроками");
                        }
                    }
        
                     foreach (var lessonDto in dayLessons)
                     {
                         var teacher = await _context.Teacher
                             .Where(t => t.User.Name == lessonDto.TeacherName)
                             .FirstOrDefaultAsync();
        
                         if (teacher == null)
                         {
                             return NotFound($"Учитель не найден");
                         }


                        // итератор по неделям
                        var week = await _context.Week.ToListAsync();

                        if(week == null)
                        {
                            return BadRequest("Недели не найдены");
                        }

                        foreach(var item in week)
                        {
                            var teacherConflicts = await _context.Schedule
                            .Include(s => s.Lesson)
                            .Include(s => s.Class)
                            .Where(s => s.Lesson.TeacherId == teacher.TeacherId &&
                                        s.WeekId == item.WeekId &&
                                        s.WeekDayId == weekDayId &&
                                        (lessonDto.StartTime < s.Lesson.EndTime &&
                                        lessonDto.EndTime > s.Lesson.StartTime))
                            .ToListAsync();

                            if (teacherConflicts.Any())
                            {
                                var conflict = teacherConflicts.First();
                                return BadRequest($"Учитель уже ведёт урок в это время в другом классе.");
                            }
                        }
                     }
        
                }
        
                var newLessons = new List<Lesson>();
                var newSchedules = new List<Schedule>();
        
                foreach (var dayLessons in lessonsByDay.Values)
                {
                    foreach (var lessonDto in dayLessons)
                    {
                        var subject = await _context.Subject
                            .FirstOrDefaultAsync(s => s.Name == lessonDto.SubjectName);
        
                        if(subject == null)
                        {
                            return NotFound($"Предмет не найден");
                        }
        
        
                        var teacher = await _context.Teacher
                            .Where(t => t.User.Name == lessonDto.TeacherName)
                            .FirstOrDefaultAsync();
        
                        if (teacher == null)
                        {
                            return NotFound($"Учитель не найден");
                        }

                        var room = await _context.Room
                            .Where(r => r.Name == lessonDto.RoomName)
                            .FirstOrDefaultAsync();

                        if (room == null)
                        {
                            return NotFound($"Кабинет не найден");
                        }

                        // итератор на неделю 
                        var week = await _context.Week.ToListAsync();

                        if (week == null)
                        {
                            return BadRequest("Недели не найдены");
                        }

                        foreach (var item in week)
                        {
                            var newLesson = new Lesson
                            {
                                LessonId = Guid.NewGuid(),
                                SubjectId = subject.SubjectId,
                                TeacherId = teacher.TeacherId,
                                StartTime = lessonDto.StartTime,
                                EndTime = lessonDto.EndTime,
                                RoomId = room.RoomId
                            };
                            newLessons.Add(newLesson);

                            newSchedules.Add(new Schedule
                            {
                                ScheduleId = Guid.NewGuid(),
                                ClassId = classEntity.ClassId,
                                WeekDayId = lessonDto.WeekDayId,
                                LessonId = newLesson.LessonId,
                                WeekId = item.WeekId,
                                LessonOrder = lessonDto.LessonOrder
                            });
                        }
                    }
                }
        
                await _context.Lesson.AddRangeAsync(newLessons);
                await _context.Schedule.AddRangeAsync(newSchedules);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
        
                 return Ok(new
                 {
                     message = "Расписание добавлено."
                 });
        
                //return CreatedAtAction(nameof(GetSchedule), new { classId = classEntity.ClassId }, newSchedules);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Ошибка при добавлении расписания.",
                    Details = dbEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Непредвиденная ошибка!",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Удаление урока из расписания
        /// </summary>
        /// <param name="className"></param>
        /// <param name="weekId"></param>
        /// <param name="weekDayId"></param>
        /// <param name="lessonOrder"></param>
        /// <returns></returns>
        [HttpDelete("ByClassLesson/{className}/{weekId}/{weekDayId}/{lessonOrder}")]
        public async Task<IActionResult> DeleteScheduleByClassLesson(string className, int weekId, int weekDayId, int lessonOrder)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var classEntity = await _context.Class.FirstOrDefaultAsync(c => c.Name == className);
                if (classEntity == null)
                {
                    return NotFound($"Класс '{className}' не найден.");
                }

                var schedule = await _context.Schedule
                    .FirstOrDefaultAsync(s =>
                        s.ClassId == classEntity.ClassId &&
                        s.WeekId == weekId &&
                        s.WeekDayId == weekDayId &&
                        s.LessonOrder == lessonOrder);

                if (schedule == null)
                {
                    return NotFound("Запись в расписании не найдена.");
                }

                var lessonId = schedule.LessonId;

                _context.Schedule.Remove(schedule);

                bool isLessonUsedElsewhere = await _context.Schedule
                    .AnyAsync(s => s.LessonId == lessonId && s.ScheduleId != schedule.ScheduleId);

                if (!isLessonUsedElsewhere)
                {
                    var lesson = await _context.Lesson.FindAsync(lessonId);
                    if (lesson != null)
                    {
                        _context.Lesson.Remove(lesson);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new
                {
                    Message = "Ошибка при удалении",
                    Details = ex.Message
                });
            }
        }

        /// Делал Слава (нужно будет проверить)
        /// <summary>
        /// Обновляет конкретную запись расписания по её идентификатору.
        /// </summary>
        /// <param name="scheduleId">Уникальный идентификатор записи расписания, которую нужно обновить.</param>
        /// <param name="editingDto">Объект DTO, содержащий новый идентификатор урока и порядковый номер.</param>
        /// <returns></returns>
        //[HttpPut("{scheduleId}")]
        //public async Task<IActionResult> EditingSchedule(Guid scheduleId, [FromBody] LessonForAddScheduleDto editingDto)
        //{
        //    try
        //    {
        //        var scheduleEntry = await _context.Schedule
        //            .Include(s => s.Lesson)
        //            .FirstOrDefaultAsync(s =>
        //            s.ScheduleId == scheduleId &&
        //            s.WeekId == editingDto.WeekId);
        //
        //        if (scheduleEntry == null)
        //        {
        //            return NotFound($"Запись расписания по ID {scheduleId} не найдена.");
        //        }
        //
        //        var lesson = await _context.Lesson
        //            .FirstOrDefaultAsync(l => l.LessonId == editingDto.LessonId);
        //
        //        if (lesson == null)
        //        {
        //            return NotFound($"Урок по ID {editingDto.LessonId} не найден.");
        //        }
        //
        //        scheduleEntry.LessonId = editingDto.LessonId;
        //        scheduleEntry.LessonOrder = editingDto.LessonOrder;
        //
        //        await _context.SaveChangesAsync();
        //
        //        return Ok(new
        //        {
        //            Message = "Редактирование расписания прошло успешно",
        //            ScheduleId = scheduleId
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new
        //        {
        //            Message = "Ошибка при редактировании расписания",
        //            Details = ex.Message
        //        });
        //    }
        //n
        //}
    }
}


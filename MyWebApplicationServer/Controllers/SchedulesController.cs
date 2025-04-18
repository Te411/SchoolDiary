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
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetSchedule()
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

            var result = schedule
                .GroupBy(s => new { s.WeekDayId, s.WeekDay.Name, s.Class.ClassId})
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
                        Homework = s.Lesson.Homework ?? null,
                        Room = s.Lesson.Room?.Trim()
                    }).ToList()
                })
                .ToList();

            return result;
        }

        /// <summary>
        /// Получить расписание по id класса
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpGet("ByClassId/{classId}")]
        public async Task<ActionResult<List<ScheduleDto>>> GetSchedule(Guid classId)
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
                        Homework = s.Lesson.Homework ?? null,
                        Room = s.Lesson.Room?.Trim()
                    }).ToList()
                })
                .ToList();

            return result;
        }

        ///// <summary>
        ///// GET: api/ByClassName/Schedules/5
        ///// Получить расписание по имени класса
        ///// </summary>
        ///// <param name="className"></param>
        ///// <returns></returns>
        //[HttpGet("ByClassName/{className}")]
        //public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedule(string className)
        //{
        //    var schedule = await _context.Schedule
        //        .Where(s => s.Class.Name == className)
        //        .Include(s => s.Class)
        //        .Include(s => s.WeekDay)
        //        .Include(s => s.Lesson)
        //            .ThenInclude(l => l.Subject)
        //        .Include(s => s.Lesson)
        //            .ThenInclude(l => l.Teacher)
        //                .ThenInclude(t => t.User)
        //        .OrderBy(s => s.WeekDayId)
        //        .ThenBy(s => s.LessonOrder)
        //        .ToListAsync();

        //    if (schedule == null || !schedule.Any())
        //    {
        //        return NotFound();
        //    }

        //    return schedule;
        //}

        /// <summary>
        /// Получить расписание по имени класса
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
                        Homework = s.Lesson.Homework ?? null,
                        Room = s.Lesson.Room?.Trim()
                    }).ToList()
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
                var classEntity = await _context.Class
                    .FirstOrDefaultAsync(c => c.Name == addScheduleDto.ClassName);
       
                if (classEntity == null)
                {
                    return NotFound($"Класса с именем '{addScheduleDto.ClassName}' не существует!");
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
                        Message = $"Уроки с ID: {string.Join(", ", missingIds)} не найдены."
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
                    .Where(s => s.ClassId == classEntity.ClassId && s.WeekDayId == addScheduleDto.WeekDayId)
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
                   return NotFound($"Класса с именем '{addScheduleDto.ClassName}' не существует!");
               }
       
               var lessonsByDay = addScheduleDto.Lessons
                   .GroupBy(l => l.WeekDayId)
                   .ToDictionary(g => g.Key, g => g.ToList());
       
               foreach (var (weekDayId, dayLessons) in lessonsByDay)
               {
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
                           return NotFound($"Предмет {lessonDto.SubjectName} не найден");
                       }
       
                       var teacher = await _context.Teacher
                           .FirstOrDefaultAsync(t => t.TeacherId == lessonDto.TeacherId);
       
                       if (teacher == null)
                       {
                           return NotFound($"Учитель с ID {lessonDto.TeacherId} не найден");
                       }
       
                       var newLesson = new Lesson
                       {
                           LessonId = Guid.NewGuid(),
                           SubjectId = subject.SubjectId,
                           TeacherId = lessonDto.TeacherId,
                           StartTime = lessonDto.StartTime,
                           EndTime = lessonDto.EndTime,
                           Homework = lessonDto.Homework ?? null,
                           Room = lessonDto.Room?.Trim()
                       };
                       newLessons.Add(newLesson);
       
                       newSchedules.Add(new Schedule
                       {
                           ScheduleId = Guid.NewGuid(),
                           ClassId = classEntity.ClassId,
                           WeekDayId = lessonDto.WeekDayId,
                           LessonId = newLesson.LessonId,
                           LessonOrder = lessonDto.LessonOrder
                       });
                   }
               }
       
               await _context.Lesson.AddRangeAsync(newLessons);
               await _context.Schedule.AddRangeAsync(newSchedules);
               await _context.SaveChangesAsync();
               await transaction.CommitAsync();
       
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
        //            .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);
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
        //
        //}
    }
}


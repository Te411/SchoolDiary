using MyWebApplicationServer.DTOs.Lesson;

namespace MyWebApplicationServer.DTOs.Schedule
{
    public class ScheduleWeekDayByTeacherDto
    {
        /// <summary>
        /// День недели
        /// </summary>
        public string WeekDayName { get; set; }

        /// <summary>
        /// Список уроков
        /// </summary>
        public List<LessonForScheduleByTeacherDto> Lessons { get; set; }
    }
}

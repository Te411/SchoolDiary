using MyWebApplicationServer.DTO.Lesson;

namespace MyWebApplicationServer.DTO.Schedule
{
    public class ScheduleWeekDayByClassDto
    {
        /// <summary>
        /// День недели
        /// </summary>
        public string WeekDayName { get; set; }

        /// <summary>
        /// Список уроков
        /// </summary>
        public List<LessonForScheduleByClassDto> Lessons { get; set; }
    }
}

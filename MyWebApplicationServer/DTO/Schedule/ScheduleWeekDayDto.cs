using MyWebApplicationServer.DTO.Lesson;

namespace MyWebApplicationServer.DTO.Schedule
{
    public class ScheduleWeekDayDto
    {
        /// <summary>
        /// День недели
        /// </summary>
        public string WeekDayName { get; set; }

        /// <summary>
        /// Список уроков
        /// </summary>
        public List<LessonForScheduleDto> Lessons { get; set; }
    }
}

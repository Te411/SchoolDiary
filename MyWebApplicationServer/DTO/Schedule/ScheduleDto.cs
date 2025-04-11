using MyWebApplicationServer.DTO.Lesson;

namespace MyWebApplicationServer.DTO.Schedule
{
    public class ScheduleDto
    {
        /// <summary>
        /// День недели
        /// </summary>
        public string WeekDayName { get; set; }

        /// <summary>
        /// Список уроков
        /// </summary>
        public List<LessonDto> Lessons { get; set; }
    }
}

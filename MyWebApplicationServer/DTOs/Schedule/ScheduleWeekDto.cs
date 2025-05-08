using MyWebApplicationServer.DTOs.Lesson;

namespace MyWebApplicationServer.DTOs.Schedule
{
    public class ScheduleWeekDto
    {
        /// <summary>
        /// Номер недели
        /// </summary>
        public int WeekId { get; set; }

        /// <summary>
        /// Дата начала недели
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата конца недели
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Список уроков
        /// </summary>
        public List<ScheduleWeekDayDto> Schedule { get; set; }
    }
}

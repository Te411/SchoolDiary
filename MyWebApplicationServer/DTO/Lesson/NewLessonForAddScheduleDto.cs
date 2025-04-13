namespace MyWebApplicationServer.DTO.Lesson
{
    public class NewLessonForAddScheduleDto
    {
        /// <summary>
        /// Уникальный индентификатор дня недели
        /// </summary>
        public int WeekDayId { get; set; }

        /// <summary>
        /// Порядковый номер урока
        /// </summary>
        public int LessonOrder { get; set; }

        /// <summary>
        /// Уникальный идентификатор предмета
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Уникальный идентификатор учителя
        /// </summary>
        public Guid TeacherId { get; set; }

        /// <summary>
        /// Начало урока
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Конец урока
        /// </summary>
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Домашнее задание
        /// </summary>
        public string? Homework { get; set; }

        /// <summary>
        /// Кабинет
        /// </summary>
        public string? Room { get; set; }
    }
}
